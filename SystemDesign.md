# System Design Interview — Complete Guide (Basic → Advanced)

A structured roadmap from fundamentals to advanced distributed systems, with an interview framework, common problems, and trade-off discussions. Examples often refer to fintech/remittance workloads (transactions, AML, payment routing) since that aligns with real-world patterns.

---

## Table of Contents

1. [How to Use This Guide](#1-how-to-use-this-guide)
2. [Part 1 — Foundations (Must Know)](#part-1--foundations-must-know)
3. [Part 2 — Core Building Blocks](#part-2--core-building-blocks)
4. [Part 3 — Data Layer Deep Dive](#part-3--data-layer-deep-dive)
5. [Part 4 — Communication & Messaging](#part-4--communication--messaging)
6. [Part 5 — Distributed Systems Concepts](#part-5--distributed-systems-concepts)
7. [Part 6 — Architectural Patterns](#part-6--architectural-patterns)
8. [Part 7 — Reliability, Observability & Security](#part-7--reliability-observability--security)
9. [Part 8 — The Interview Framework (SNAKE / RESHADED)](#part-8--the-interview-framework)
10. [Part 9 — Back-of-Envelope (Capacity Estimation)](#part-9--back-of-envelope-capacity-estimation)
11. [Part 10 — Classic Problems Walkthrough](#part-10--classic-problems-walkthrough)
12. [Part 11 — Trade-off Cheatsheet](#part-11--trade-off-cheatsheet)
13. [Part 12 — 6-Week Preparation Plan](#part-12--6-week-preparation-plan)
14. [Part 13 — Resources](#part-13--resources)

---

## 1. How to Use This Guide

System design interviews test three things:

1. **Breadth of vocabulary** — do you know the building blocks and their trade-offs?
2. **Structured thinking** — can you drive a 45-minute conversation from vague prompt to concrete design?
3. **Judgment** — do you pick the right tool and defend it against alternatives?

Work the guide in order. Don't memorize — internalize the *why* behind each choice. Every section ends with the trade-offs you must be able to articulate out loud.

---

# Part 1 — Foundations (Must Know)

## 1.1 What is System Design?

Designing a system means deciding how independent components collaborate to meet **functional requirements** (what it does) under **non-functional requirements / NFRs** (how well it does it — latency, availability, throughput, cost, consistency, durability, security).

Always separate the two early in an interview.

## 1.2 The Core Vocabulary

| Term | Meaning | Typical target |
|---|---|---|
| **Latency** | Time for one request to complete | p50/p95/p99 — always think in percentiles, never average |
| **Throughput** | Requests or bytes per second | RPS, QPS, MB/s |
| **Availability** | % of time the system answers correctly | 99.9% (3-nines) = ~8.7 hrs/year downtime; 99.99% = ~52 min/year |
| **Reliability** | Probability of correct behavior over time | Linked to MTBF / MTTR |
| **Durability** | Probability that stored data survives | "11 nines" for S3 |
| **Consistency** | All nodes see the same data at the same time | Strong, sequential, causal, eventual |
| **Fault tolerance** | System keeps working when components fail | Redundancy + failover |
| **Scalability** | Handle more load by adding resources | Vertical vs horizontal |

## 1.3 Scaling: Vertical vs Horizontal

- **Vertical (scale up)** — bigger machine: more CPU/RAM/SSD. Simple, no code changes, but has a ceiling, a single point of failure, and gets expensive fast.
- **Horizontal (scale out)** — more machines behind a load balancer. Near-infinite ceiling but introduces distributed-systems problems: consistency, partitioning, coordination.

**Rule:** Scale vertically until you can't, then scale horizontally. The transition is the hard part.

## 1.4 CAP Theorem

In a network partition, a distributed system can guarantee only **two of three**: **C**onsistency, **A**vailability, **P**artition tolerance. Since partitions are inevitable in real networks, you actually choose between **CP** and **AP**.

- **CP systems:** HBase, MongoDB (configurable), Zookeeper, etcd, traditional RDBMS with sync replication. Reject reads/writes during partitions to stay consistent.
- **AP systems:** Cassandra, DynamoDB, CouchDB, Riak. Serve stale data during partitions; reconcile later.

**Extension — PACELC:** If there's a Partition, choose A or C; Else (normal operation), choose Latency or Consistency. This is a more useful framing.

For a remittance ledger you almost always want **CP** — you cannot double-spend. For a product catalog, **AP** is fine.

## 1.5 ACID vs BASE

**ACID** (traditional RDBMS — SQL Server, Postgres, Oracle):
- **A**tomicity — all-or-nothing transactions
- **C**onsistency — constraints always hold
- **I**solation — concurrent txns don't interfere
- **D**urability — committed data survives crashes

**BASE** (many NoSQL):
- **B**asically **A**vailable
- **S**oft state
- **E**ventually consistent

Payment and AML workloads almost always demand ACID at the system of record. Use BASE for caches, analytics, feeds, recommendations.

## 1.6 Consistency Models (from strongest to weakest)

1. **Linearizable / Strong** — every read sees the latest write. Most expensive.
2. **Sequential** — all nodes see operations in the same order (not necessarily real-time).
3. **Causal** — if A causes B, everyone sees A before B.
4. **Read-your-writes** — you always see your own writes.
5. **Monotonic reads** — you never see time go backwards.
6. **Eventual** — given no new writes, all replicas converge.

---

# Part 2 — Core Building Blocks

## 2.1 DNS

Translates domain → IP. Cached at browser, OS, resolver. In design discussions, mention **GeoDNS** (route users to nearest region) and **DNS-based load balancing** (round-robin, weighted).

## 2.2 CDN (Content Delivery Network)

Edge servers cache static assets close to users. Two modes:
- **Push** — you upload; CDN stores.
- **Pull** — CDN fetches from origin on first miss, caches with TTL.

Modern CDNs (Cloudflare, Fastly) also do edge compute, DDoS protection, TLS termination, WAF. For a remittance portal, CDN serves JS/CSS/images and shields the origin from bots.

## 2.3 Load Balancers

Distribute traffic across backend instances.

**Layer 4 (Transport)** — operates on TCP/UDP. Fast, no content inspection. Example: AWS NLB.
**Layer 7 (Application)** — operates on HTTP. Can route by URL, header, cookie; do TLS termination, compression, rewrites. Example: NGINX, HAProxy, AWS ALB, Ocelot Gateway.

**Algorithms:**
- Round-robin
- Weighted round-robin
- Least connections
- Least response time
- IP hash / consistent hash (for session affinity)
- Power of two choices (pick 2 random, choose the less loaded — surprisingly good)

**Sticky sessions:** LB routes a client to the same backend. Avoid if possible — breaks horizontal scaling and failover. Externalize session state to Redis instead.

## 2.4 Caching

Caching is the #1 performance tool. Know it cold.

**Where to cache:**
- Browser (HTTP headers: `Cache-Control`, `ETag`)
- CDN
- API Gateway
- Application (in-process — fastest, smallest)
- Distributed cache (Redis, Memcached)
- Database query cache

**Write strategies:**
| Strategy | How it works | When to use |
|---|---|---|
| **Cache-aside (lazy)** | App reads cache → on miss reads DB and populates cache | Default choice. Read-heavy workloads. |
| **Write-through** | App writes to cache, cache writes to DB synchronously | Strong consistency between cache and DB; slower writes |
| **Write-behind (write-back)** | App writes to cache, cache flushes to DB async | Write-heavy, can tolerate small data-loss window |
| **Write-around** | App writes directly to DB, cache populated on read miss | Writes that are rarely re-read immediately |
| **Refresh-ahead** | Cache proactively refreshes hot keys before TTL | Predictable hot keys, low latency SLO |

**Eviction:** LRU (default), LFU, FIFO, TTL-based, random.

**Pitfalls to mention in interviews:**
- **Cache stampede / thundering herd** — many clients miss simultaneously. Mitigate with request coalescing, staggered TTLs, probabilistic early expiration, or a distributed lock on refresh.
- **Cache penetration** — queries for non-existent keys bypass cache and hammer DB. Mitigate with null-value caching or a Bloom filter.
- **Cache avalanche** — many keys expire at once. Mitigate with jittered TTL.
- **Hot key** — one key saturates a single cache node. Mitigate with local L1 cache in front of Redis, or key sharding.

## 2.5 API Gateway

Single entry point for external clients. Responsibilities:
- Routing to microservices
- AuthN / AuthZ (JWT, OAuth2)
- Rate limiting & quotas
- Request/response transformation
- TLS termination
- Logging, metrics, tracing injection

You already use **Ocelot** for this in RemitERP. Others: Kong, NGINX+, Envoy, AWS API Gateway, Azure API Management.

---

# Part 3 — Data Layer Deep Dive

## 3.1 SQL vs NoSQL

**Pick SQL (Postgres, SQL Server, MySQL) when:**
- Data is highly relational, requires joins
- You need ACID transactions (money, inventory, orders)
- Schema is stable
- Complex ad-hoc queries needed

**Pick NoSQL when:**
- Schema is flexible or evolving
- Horizontal write scale needed beyond one master
- Access pattern is simple key lookup or document fetch
- Eventual consistency is acceptable

**NoSQL families:**
| Family | Example | Best for |
|---|---|---|
| Key-Value | Redis, DynamoDB, Riak | Caching, sessions, feature flags |
| Document | MongoDB, Couchbase | JSON-shaped data, flexible schema |
| Wide-column | Cassandra, HBase, ScyllaDB | Huge writes, time-series, event logs |
| Graph | Neo4j, Amazon Neptune | Relationships (fraud rings, social) |
| Time-series | InfluxDB, TimescaleDB | Metrics, IoT telemetry |
| Search | Elasticsearch, OpenSearch | Full-text, log search, aggregation |

For remittance: transaction ledger → SQL Server (ACID). Audit logs & search → Elasticsearch. Rate limits & idempotency keys → Redis. AML screening graph → Neo4j or a custom graph index.

## 3.2 Indexing

- **B-Tree** — default for range queries and equality. What most RDBMS indexes use.
- **Hash index** — O(1) equality, no range scans.
- **LSM-tree** — write-optimized (Cassandra, RocksDB, LevelDB).
- **Inverted index** — full-text search (Elasticsearch, Lucene).

**Rules:**
- Indexes speed reads, slow writes, consume space.
- Composite indexes follow left-prefix rule — `(country, created_at)` can serve `WHERE country=?` but not `WHERE created_at=?`.
- **SARGable** predicates (Search ARGument-able) use indexes; wrapping a column in a function (`WHERE YEAR(created_at)=2025`) breaks that. You already hit this on the TM Monitoring pivot report.
- **Covering index** — includes all columns a query needs so the engine never touches the base table.

## 3.3 Replication

Replicas = copies of the data on different nodes.

**Primary–Secondary (leader–follower):**
- Writes go to primary; reads can go to either.
- **Synchronous replication** — writes block until replica acks. Strong consistency, higher latency, risk of write stalls if replica is slow.
- **Asynchronous replication** — primary acks immediately. Low latency, risk of data loss on failover, replica lag.
- **Semi-synchronous** — at least one replica must ack (MySQL, PostgreSQL).

**Multi-leader / Multi-master** — multiple primaries accept writes. Higher availability and local writes across regions, but you must resolve write conflicts (last-write-wins, CRDTs, application-specific merge).

**Leaderless (Dynamo-style)** — any node accepts writes, quorum-based reads. Used by Cassandra, DynamoDB. Read + Write quorums: `R + W > N` for strong consistency.

## 3.4 Partitioning / Sharding

Split data across nodes so no single node holds everything.

**Strategies:**
- **Range-based** — shard by ranges of a key (e.g. user_id 1–1M on shard 1). Simple, but hot spots if traffic skews to one range.
- **Hash-based** — `hash(key) mod N` → shard. Even distribution, but resharding when you add/remove nodes rehashes everything.
- **Consistent hashing** — nodes and keys are placed on a ring; adding/removing a node only reshuffles ~1/N keys. Used by DynamoDB, Cassandra, Memcached clients.
- **Directory-based** — a lookup service maps key → shard. Flexible, but the directory becomes a SPOF.
- **Geo-based** — shard by user region, keep data near users (critical for GDPR / data residency).

**Shard key selection — most important decision:**
- High cardinality (many distinct values)
- Even distribution (no hot partitions)
- Aligned with common query patterns (otherwise you scatter-gather)
- Immutable (changing a shard key means moving data)

For remittance, you might shard transactions by `sender_country + YYYYMM` to keep regional compliance data local and queries efficient.

## 3.5 Normalization vs Denormalization

- **Normalized** — minimal redundancy, changes update one place. Joins get expensive at scale.
- **Denormalized** — redundant data embedded for read speed. Writes must update multiple places; risk of drift.

NoSQL usually forces denormalization because joins aren't native. Read-heavy workloads often denormalize even in SQL (materialized views, summary tables).

## 3.6 Data Warehouse vs OLTP

- **OLTP** (online transaction processing) — short, frequent txns; row-store; normalized. Your production DB.
- **OLAP** (online analytical processing) — long scans, aggregations; column-store (Snowflake, BigQuery, Redshift, ClickHouse). Populated via ETL/ELT from OLTP.

Don't run BI queries on OLTP — you'll kill production.

---

# Part 4 — Communication & Messaging

## 4.1 Synchronous vs Asynchronous

- **Sync** — caller waits for response (HTTP request, gRPC call). Simple, but failures cascade.
- **Async** — caller fires a message and continues; response comes later (or never). Decouples services, smooths bursts, isolates failures.

Rule of thumb: use sync for user-facing read paths; async for side-effects, long-running work, cross-service workflows.

## 4.2 REST vs GraphQL vs gRPC

| | REST | GraphQL | gRPC |
|---|---|---|---|
| Protocol | HTTP/JSON | HTTP/JSON | HTTP/2 + Protobuf |
| Contract | Loose (OpenAPI optional) | Strongly typed schema | Strongly typed `.proto` |
| Over/under-fetch | Yes | No — client asks for exactly what it needs | N/A, tight schema |
| Tooling/caching | Mature HTTP caching | Harder to cache | Streaming, bidi |
| Best for | Public APIs, CRUD | Complex UIs, aggregating services | Internal service-to-service |

## 4.3 Message Queues & Streaming

**Queues (point-to-point):** RabbitMQ, AWS SQS, Azure Service Bus. Each message is consumed by one consumer. Great for work distribution.

**Streams (pub-sub + replay):** Kafka, AWS Kinesis, Redis Streams, Pulsar. Messages are durable logs; multiple consumer groups replay independently. Great for event sourcing, CDC, analytics pipelines.

**Delivery guarantees:**
- **At-most-once** — may lose messages. Cheap. (Fire-and-forget.)
- **At-least-once** — may duplicate. Most common. Consumer must be **idempotent**.
- **Exactly-once** — rare, expensive. Kafka offers effectively-once via transactions + idempotent producers.

**Patterns to know:**
- **Dead-letter queue (DLQ)** — poison messages go here for manual inspection.
- **Outbox pattern** — write business state and outgoing event in the same DB transaction; a relay publishes from the outbox table. Prevents "wrote to DB but failed to publish" drift.
- **Saga pattern** — long-running distributed transaction as a chain of local transactions + compensating actions. Choreography (events) vs Orchestration (central coordinator).
- **CDC (Change Data Capture)** — stream DB changes into Kafka (Debezium, SQL Server CDC).

You already use Redis Pub/Sub and Hangfire — Hangfire is essentially a persistent job queue with retries.

## 4.4 Idempotency

An operation is idempotent if doing it twice has the same effect as doing it once. In distributed systems you WILL get duplicate deliveries, retries, and network timeouts with unknown outcomes.

**Techniques:**
- **Idempotency keys** — client sends a UUID; server stores the result keyed by it and returns the cached result on retry.
- **Database uniqueness constraints** — reject duplicate inserts.
- **State machines** — only allow transitions from valid states; duplicate events become no-ops.
- **`sp_getapplock` / advisory locks** — serialize a critical section across a cluster.

Your RemitERP duplicate-transaction work already uses several of these.

---

# Part 5 — Distributed Systems Concepts

## 5.1 Consistent Hashing

Place N servers on a ring of hash values (e.g., 0 to 2³²). Each key hashes to a point; it's owned by the next server clockwise. Adding a server only reassigns keys between that server and its neighbor — about K/N keys instead of K.

**Virtual nodes:** each physical server owns many points on the ring → smoother distribution, easier to add weighted capacity. Used by Cassandra, Dynamo, memcached clients, Envoy.

## 5.2 Quorum

With N replicas, a **write quorum W** must ack a write and a **read quorum R** must be consulted on read. `R + W > N` guarantees a read sees the latest write.
- `R=1, W=N` — fast reads, slow writes, high consistency.
- `R=N, W=1` — fast writes, slow reads.
- `R=W=(N/2)+1` — balanced (Dynamo default).

## 5.3 Leader Election

When a leader is needed (e.g., primary DB, cluster coordinator), nodes must agree on one. Algorithms: **Raft**, **Paxos**, **Zab** (Zookeeper), **Bully**. Most engineers delegate to Zookeeper / etcd / Consul rather than implement it.

## 5.4 Consensus Algorithms

**Paxos** — the original; correct but famously hard to understand.
**Raft** — designed to be understandable. Used by etcd, Consul, CockroachDB. Concepts: leader, term, log replication, commit index. Know the high-level flow.

**Two-Phase Commit (2PC):**
- Prepare phase — coordinator asks all participants "can you commit?"
- Commit phase — if all say yes, coordinator tells them to commit.
Problems: blocking if coordinator fails, slow, doesn't handle partitions well. Still used in distributed RDBMS, but Saga has largely replaced it for microservices.

## 5.5 Distributed Transactions

Two options for consistency across services:
- **2PC** — strong consistency, blocking, not scalable. Fine inside a single DB cluster.
- **Saga** — eventual consistency via compensations. Scalable, more complex error handling.

Remittance example (Saga): Debit sender → Debit partner settlement account → Credit receiver. If the third step fails, compensate: refund partner, refund sender.

## 5.6 Clocks & Ordering

**Wall-clock time is unreliable** across machines — NTP skew, leap seconds. Don't build correctness on it.

- **Lamport timestamps** — logical counter; gives you a total order of causally related events.
- **Vector clocks** — detect concurrent events; used by Dynamo, Riak.
- **Hybrid Logical Clocks (HLC)** — wall-clock + logical counter; used by CockroachDB, MongoDB.
- **TrueTime** — Google Spanner's bounded-uncertainty clock backed by GPS + atomic clocks. Enables externally consistent global transactions.

## 5.7 Gossip Protocol

Nodes periodically exchange state with a few random peers. Information spreads exponentially. Used for cluster membership, failure detection (Cassandra, Consul, Redis Cluster). Cheap, fault-tolerant, eventually converges.

## 5.8 Bloom Filter

Probabilistic set-membership structure. "Definitely not in set" or "probably in set." Zero false negatives, tunable false positives. Uses a bit array + k hash functions.

Use cases: skip disk reads in LSM-trees (Cassandra, RocksDB), cache-penetration defense, deduplication, malicious-URL filters.

## 5.9 Count-Min Sketch / HyperLogLog

Probabilistic counters.
- **HyperLogLog** — cardinality (unique count) in a tiny memory footprint. Redis `PFCOUNT`.
- **Count-Min Sketch** — frequency of items. Heavy hitters, trend detection.

---

# Part 6 — Architectural Patterns

## 6.1 Monolith vs Microservices vs Modular Monolith

**Monolith:** One deployable. Simple, fast local dev, one DB, easy transactions. Scales until the team and codebase get too big.

**Microservices:** Many small services, each with its own DB. Independent deploys, polyglot, isolated failures — but distributed-systems tax: network, observability, consistency, versioning, deployment complexity.

**Modular monolith:** Strong internal module boundaries inside one deployable. The current "default good answer" for most teams — get the boundaries right first, split later if load demands it.

**When to split into microservices:**
- Different scaling needs per domain
- Team autonomy / independent release cadences
- Different tech stacks required
- Large enough team that coordination in one repo becomes the bottleneck

## 6.2 Event-Driven Architecture

Services emit domain events; other services subscribe. Produces loose coupling and natural audit trails. Needs a broker (Kafka/RabbitMQ/Service Bus) and discipline around schemas.

## 6.3 CQRS (Command Query Responsibility Segregation)

Separate the write model (commands) from the read model (queries). Writes produce events that update denormalized read projections optimized for each query. Pairs well with Event Sourcing.

Good for: complex domains with very different read vs write patterns (reporting dashboards, search).
Bad for: simple CRUD — pure overhead.

## 6.4 Event Sourcing

Store every state change as an immutable event. Current state = replay of events. Audit log is free; time travel is possible; projections can be rebuilt.

Costs: schema evolution is hard, snapshots needed for performance, developer mental model shift.

Perfect fit for a remittance ledger — you already have most of it conceptually.

## 6.5 Microservice Patterns

- **API Gateway** — single entry point (Ocelot)
- **Backend for Frontend (BFF)** — a gateway per client type (web/mobile)
- **Service Discovery** — Consul, Eureka, Kubernetes DNS
- **Sidecar** — ancillary process deployed alongside the main service (Envoy for service mesh)
- **Circuit Breaker** — stop calling a failing dependency to give it time to recover (Polly in .NET, Hystrix historically)
- **Bulkhead** — isolate resources so one failure can't drown others (separate thread pools / connection pools)
- **Retry with exponential backoff + jitter** — never retry in a tight loop; never retry non-idempotent calls without idempotency keys
- **Timeouts** — always. A missing timeout is a hanging thread.
- **Strangler Fig** — gradually replace a monolith by routing new features to new services behind the gateway
- **Anti-corruption Layer** — translation boundary when integrating with legacy systems

---

# Part 7 — Reliability, Observability & Security

## 7.1 Reliability Techniques

- **Redundancy** — N+1 or N+2 instances, multi-AZ, multi-region.
- **Health checks** — liveness (am I alive?) vs readiness (am I ready to serve?).
- **Graceful degradation** — serve stale data, disable non-critical features rather than fail fully.
- **Chaos engineering** — inject failures in prod (Netflix Chaos Monkey) to find weaknesses before they find you.
- **Blue/green deployment** — two full environments, switch traffic instantly; easy rollback.
- **Canary deployment** — route small % of traffic to new version, watch metrics, ramp up.
- **Feature flags** — deploy code dark, enable per-cohort, kill instantly without redeploy.

## 7.2 Rate Limiting

Protect systems from abuse and bursty load.

**Algorithms:**
- **Fixed window** — simple but has boundary burst (2x rate at window edge).
- **Sliding window log** — accurate, memory-heavy.
- **Sliding window counter** — approximation of log with much less memory.
- **Token bucket** — tokens refill at a fixed rate; request consumes a token. Allows bursts up to bucket size. Most common.
- **Leaky bucket** — requests enter a queue, processed at constant rate. Smooths output.

Implement in Redis (atomic `INCR` + TTL, or Lua scripts for token bucket). Apply at gateway, per-user and per-IP.

## 7.3 Observability — the Three Pillars

1. **Metrics** — aggregated numbers over time (Prometheus, Datadog, Azure Monitor). RED: Rate/Errors/Duration. USE: Utilization/Saturation/Errors.
2. **Logs** — structured event records (Serilog → ELK, Seq, Loki). You already ship to App_Data/Logs with daily rolling. For a distributed system, centralize logs.
3. **Traces** — follow a single request across services (OpenTelemetry, Jaeger, Zipkin, Application Insights). Correlation IDs end-to-end.

Add **SLIs / SLOs / error budgets** as senior-level vocabulary:
- **SLI** — a measured indicator (p99 latency, error rate).
- **SLO** — target for an SLI (99.9% of requests < 300ms).
- **Error budget** — 1 − SLO. Spend it on feature velocity; ship less when you're burning it.

## 7.4 Security Essentials

- **TLS everywhere** — internal too, not just edge.
- **Authentication** — who are you? (OAuth 2.0, OIDC, JWT, ASP.NET Identity)
- **Authorization** — what can you do? (RBAC, ABAC, policy engines like OPA)
- **Secrets management** — never in code/config. Use Azure Key Vault, AWS Secrets Manager, HashiCorp Vault.
- **Input validation** — on every boundary. Parameterized queries always.
- **OWASP Top 10** — injection, broken auth, sensitive data exposure, XXE, broken access control, misconfiguration, XSS, insecure deserialization, vulnerable components, insufficient logging.
- **PII handling / GDPR** — minimization, retention, right to erasure, audit. For RemitERP you've already got the AML/GDPR retention tension — classic interview discussion point.
- **Encryption** — at rest (TDE, disk encryption) and in transit. KMS-managed keys.
- **Defense in depth** — WAF + API gateway + service-level auth + DB least-privilege.

---

# Part 8 — The Interview Framework

Use a consistent structure; it signals seniority. Variations: **SNAKE**, **RESHADED**, **PEDALS**. Here's a unified 7-step version.

## Step 1 — Clarify Requirements (5 min)

**Functional:** What are the top 3–5 features we must support? What's out of scope?

**Non-functional:**
- How many users? DAU/MAU? Peak RPS?
- Read/write ratio?
- Latency target (p99)?
- Availability target?
- Consistency requirement per feature?
- Durability requirement?
- Global or regional?
- Cost sensitivity?

**Never start drawing without these answers.** If the interviewer is vague, propose numbers yourself and confirm.

## Step 2 — Capacity Estimation (5 min)

Back-of-envelope math to justify later choices. See Part 9.

## Step 3 — API Design (5 min)

List the key endpoints: method, path, params, response shape. Keeps things concrete and surfaces missed requirements.

```
POST /v1/transactions
  body: { senderId, receiverId, amount, currency, idempotencyKey }
  resp: { transactionId, status, estimatedCompletion }

GET  /v1/transactions/{id}
POST /v1/transactions/{id}/cancel
```

## Step 4 — High-Level Architecture (10 min)

Draw 6–10 boxes: client → CDN → LB → API gateway → services → caches → DBs → queues. Explain flow for the most important API.

## Step 5 — Data Model & Storage Choices (5 min)

Pick store per data type. Justify each (SQL for ledger, Redis for sessions, Elastic for search, S3 for blobs, Kafka for events). Discuss shard key and indexes.

## Step 6 — Deep Dive (10 min)

Interviewer will pick a component. Common picks:
- How does the write path stay consistent?
- How does the cache invalidate?
- How do you shard?
- How do you prevent duplicate writes?
- How do you handle hot keys?

Show depth on one or two — don't hand-wave.

## Step 7 — Scale, Bottlenecks, Trade-offs (5 min)

Walk the request path and identify the first thing that breaks at 10× load. Propose mitigation. Common answers: add read replicas, introduce a cache, shard the DB, move to async, add CDN.

Close by stating the **trade-offs you chose and what you'd revisit** if given more time. Interviewers love this.

---

# Part 9 — Back-of-Envelope (Capacity Estimation)

Memorize these numbers. Round aggressively.

## Powers of 10
- 1K = 10³, 1M = 10⁶, 1B = 10⁹, 1T = 10¹²
- 1 day ≈ 86,400 s ≈ 10⁵ s
- 1 month ≈ 2.5 × 10⁶ s
- 1 year ≈ 3.15 × 10⁷ s

## Latency numbers every programmer should know
| Operation | Time |
|---|---|
| L1 cache reference | 0.5 ns |
| Branch mispredict | 5 ns |
| L2 cache reference | 7 ns |
| Mutex lock/unlock | 25 ns |
| Main memory reference | 100 ns |
| Compress 1KB with Zippy | 3,000 ns (3 µs) |
| Send 1KB over 1 Gbps network | 10,000 ns (10 µs) |
| Read 4KB from SSD | 150,000 ns (150 µs) |
| Read 1MB from RAM | 250,000 ns (250 µs) |
| Round trip in datacenter | 500,000 ns (500 µs) |
| Read 1MB from SSD | 1,000,000 ns (1 ms) |
| Disk seek | 10 ms |
| Read 1MB from network | 10 ms |
| Read 1MB from disk | 30 ms |
| Packet CA → Netherlands → CA | 150 ms |

## Storage sizes
- `char` 1 B, `int` 4 B, `long/double` 8 B
- UUID 16 B (binary) / 36 B (text)
- Timestamp 8 B
- Small row ≈ 100–500 B
- Tweet-size text ≈ 300 B
- Thumbnail image ≈ 50 KB
- Photo 500 KB, 4K photo 5 MB
- 1 min HD video ≈ 50 MB

## Walked example — remittance platform

Given: 10M users, 20% DAU = 2M. Each active user sends 0.5 txns/day → 1M txns/day.
- QPS average: 1M / 10⁵ = 10 txns/s. Easy.
- Peak: 10× average = 100 txns/s. Still easy for one DB.
- Transaction row ≈ 1 KB → 1 GB/day raw → ~365 GB/year → manageable on a single beefy SQL node with partitioning and archival.
- Read-heavy views (dashboards, history) ≈ 100× writes → 10K reads/s → add read replicas + Redis cache for hot accounts.

The math drives the architecture. Always.

---

# Part 10 — Classic Problems Walkthrough

Each gets a skeleton. Drill them — whiteboard them from scratch at least twice.

## 10.1 URL Shortener (TinyURL / bit.ly)

**Requirements:** Shorten URL, redirect, analytics, custom aliases, expiration.

**Scale:** 100M URLs/day written, 10B redirects/day. Redirect is ~100× write — heavy read.

**Key design points:**
- Short code generation: base62 of a distributed counter (Snowflake ID → base62), or MD5/SHA of URL + collision handling, or pre-generated pool of unused codes.
- Store: Key-value (DynamoDB/Cassandra) `short → long`. 100B per entry × 100B URLs = ~10TB.
- Redirect path: CDN + Redis cache; DB is last resort. 301 vs 302 (302 lets you re-measure).
- Analytics: async event to Kafka, aggregated in warehouse.

## 10.2 News Feed (Twitter/X, Facebook)

**Requirements:** Post, follow, view feed of people you follow, ranked by recency (and relevance).

**Key design points:**
- **Fan-out on write (push)** — when user posts, copy into each follower's feed. Fast reads, huge write amplification for celebrities (Taylor Swift problem).
- **Fan-out on read (pull)** — at feed time, fetch recent posts from everyone you follow and merge. Cheap writes, expensive reads.
- **Hybrid** — push for normal users, pull for celebrities; merge at read time.
- Storage: Cassandra for timelines; Redis cache for hot users' feeds.
- Ranking: ML service scores candidates; offline feature pipeline.

## 10.3 Chat (WhatsApp, Slack)

**Requirements:** 1:1 and group messages, delivery & read receipts, online presence, offline delivery, media.

**Key design points:**
- WebSockets for long-lived connections. Connection-manager service tracks user→server mapping in Redis.
- Message flow: sender → gateway → message service → store in DB + push to recipient's connection server → ack chain.
- Storage: messages in Cassandra partitioned by `(conversation_id, bucket_by_time)`.
- Delivery guarantee: at-least-once, idempotent client-side dedupe by message UUID.
- Media: upload to object store (S3), message carries a URL.
- Group messages: fan-out at message service; cap group size; mentions get separate notification flow.

## 10.4 Rate Limiter

**Requirements:** Limit per user/API key, distributed across N gateway nodes, low latency, correct under race conditions.

**Key design points:**
- Token bucket in Redis. Atomic Lua script: check tokens, decrement, set TTL.
- Or sliding-window counter: two windows weighted by overlap.
- Fallback: if Redis is down, fail open (allow) vs fail closed (deny) — depends on product.
- Return `429 Too Many Requests` with `Retry-After` header.

## 10.5 Payment System (very relevant to you)

**Requirements:** Charge a customer, support retries, avoid double-charge, reconcile with external PSP, refunds.

**Key design points:**
- **Idempotency keys** on every request. Store `(idempotency_key, response)` in a DB table with TTL.
- **State machine** for payment: `INITIATED → AUTHORIZED → CAPTURED → SETTLED`, with `FAILED`/`REFUNDED` branches.
- **Outbox pattern** to publish events reliably.
- **Saga** across sub-ledgers: debit sender account, credit platform clearing, credit receiver.
- **Ledger store** — append-only double-entry. Never `UPDATE` a posted transaction; issue reversal entries.
- **Reconciliation** — nightly job matches internal ledger vs PSP statements; mismatches go to an ops queue.
- **Fraud / AML** — synchronous risk check before authorization; async enrichment afterwards (Dow Jones screening, PEP, velocity).

## 10.6 Ride-Sharing (Uber)

**Requirements:** Rider requests, nearby drivers, matching, live tracking, fare calc, payment.

**Key design points:**
- Driver location updates every 4–5s → high write throughput → geo-indexed store (Redis GEO, or quadtree / S2 cells / geohash).
- Matching: search nearby drivers within radius → rank by ETA → dispatch.
- Trip state machine similar to payments.
- Map-matching, surge pricing, ETAs — separate services.
- Use Kafka for live telemetry streams.

## 10.7 Video Streaming (Netflix/YouTube)

**Requirements:** Upload, transcode, store, deliver, recommend.

**Key design points:**
- Upload to object storage → transcode pipeline (queue + workers) → multiple bitrates (HLS/DASH manifests).
- Massive CDN footprint; some providers (Netflix Open Connect) place appliances inside ISPs.
- Adaptive bitrate streaming — client measures bandwidth, switches quality.
- Metadata store (search, catalog) separate from video blobs.
- Recommendation pipeline: offline batch (Spark) + online features.

## 10.8 Distributed Cache

**Requirements:** Low-latency KV, TTL, consistent hashing, high availability.

**Key design points:**
- Consistent hashing with virtual nodes for rebalance minimization.
- Replication factor 2–3.
- Eviction: LRU per node.
- Client-side sharding or a proxy (Twemproxy, Redis Cluster).
- Failover via gossip + automatic reconfiguration.

## 10.9 Notification Service

**Requirements:** Email, SMS, push; high throughput; dedupe; retries; user preferences.

**Key design points:**
- Accept via API → push to Kafka → channel-specific workers.
- Template service for i18n.
- Retry with exponential backoff; DLQ for poison.
- Dedupe by idempotency key or content hash + recipient in Redis.
- Quiet hours / preference service checked per message.

## 10.10 Search (Typeahead / Full-text)

**Requirements:** Fast prefix suggestions, personalized, trending.

**Key design points:**
- Trie in-memory for top-K prefix suggestions, backed by a persistence layer.
- Rebuild periodically from query logs; recent queries merged online.
- Full-text: Elasticsearch with inverted indices.

---

# Part 11 — Trade-off Cheatsheet

Memorize this. Every design answer is about trade-offs.

| Dimension | Pick A when… | Pick B when… |
|---|---|---|
| SQL vs NoSQL | Relations + ACID + complex queries | Simple access patterns + horizontal write scale |
| Strong vs eventual consistency | Money, inventory, identity | Feeds, analytics, caches |
| Sync vs async replication | No data loss tolerance | Low-latency writes, can tolerate replica lag |
| Monolith vs microservices | Small team, bounded domain | Many teams, different scaling needs per domain |
| REST vs gRPC | Public APIs, browser clients | Internal, performance-critical |
| Cache-aside vs write-through | Read-heavy, OK with stale | Writes must be cache-consistent |
| Push vs pull fan-out | Most users have few followers | Celebrity users with millions of followers (use hybrid) |
| 2PC vs Saga | Single DB cluster, short txns | Cross-service, long-running |
| Shared DB per service vs DB per service | Easy joins, tight coupling OK | True service autonomy |
| Hash vs range sharding | Even load, point lookups | Range scans, time-series |
| L4 vs L7 load balancer | Raw TCP speed, no inspection | HTTP-aware routing, TLS termination |

---

# Part 12 — 6-Week Preparation Plan

Two to three hours per weekday, a mock on weekends.

**Week 1 — Foundations**
- Part 1 + Part 2 of this guide
- Read *Designing Data-Intensive Applications* (DDIA) Ch. 1–4
- Draw 5 simple architectures from memory (blog, e-commerce, URL shortener)

**Week 2 — Data Layer**
- Part 3
- DDIA Ch. 5–7 (Replication, Partitioning, Transactions)
- Practice: design URL shortener end-to-end, whiteboard twice

**Week 3 — Distributed Systems**
- Parts 4 & 5
- DDIA Ch. 8–9 (Trouble with Distributed Systems, Consistency & Consensus)
- Practice: design rate limiter and distributed cache

**Week 4 — Architecture Patterns**
- Parts 6 & 7
- Read *Microservices Patterns* (Chris Richardson) selected chapters: Saga, API Gateway, Event Sourcing
- Practice: design chat system, design news feed

**Week 5 — Domain Problems**
- Part 10
- One hard problem per day, 45 min whiteboard
- Record yourself explaining — listen back, cut filler, tighten structure

**Week 6 — Mocks & Polish**
- 4–6 mock interviews (pramp, exponent, peer)
- Drill back-of-envelope numbers daily
- Review the trade-off cheatsheet every morning

---

# Part 13 — Resources

**Books (priority order):**
1. *Designing Data-Intensive Applications* — Martin Kleppmann (must-read)
2. *System Design Interview Vol. 1 & 2* — Alex Xu (interview-format, drill through)
3. *Microservices Patterns* — Chris Richardson
4. *Building Microservices* — Sam Newman
5. *Release It!* — Michael Nygard (reliability patterns)
6. *Database Internals* — Alex Petrov

**Free online:**
- High Scalability blog (highscalability.com)
- AWS / Azure / Google Cloud architecture center case studies
- Netflix, Uber, Meta, Stripe engineering blogs
- "The Google File System," "Dynamo," "Bigtable," "MapReduce," "Spanner" papers — read in that order
- GitHub: `donnemartin/system-design-primer`

**Mock interviews:**
- Pramp (free peer-to-peer)
- Exponent, interviewing.io (paid, professional interviewers)
- Peer mocks with colleagues — cheapest and surprisingly effective

**Practice tools:**
- Excalidraw or draw.io for architecture diagrams
- Your own notes repo — rebuild each classic problem from scratch weekly

---

## Final Words

The best system design candidates aren't the ones who know the most technologies — they're the ones who:

1. **Ask before building** — requirements drive everything.
2. **Reason in numbers** — back-of-envelope first, design second.
3. **Make trade-offs explicit** — "I'm choosing X over Y because of Z, and here's what I'd revisit."
4. **Go deep on at least one component** — hand-waving every layer is the #1 red flag.
5. **Acknowledge what they don't know** — "I'd benchmark this" beats confident nonsense.

You already have the raw material from RemitERP — ACID ledgers, idempotency, Redis pub/sub, Ocelot gateway, AML sagas, compliance data residency. Translate that hands-on experience into the interview vocabulary above, and you'll present as a senior engineer, not a junior one memorizing patterns.

Good luck.
