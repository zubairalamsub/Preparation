# Networking Complete Guide - Basic to Advanced

## Table of Contents
1. [Networking Fundamentals](#networking-fundamentals)
2. [OSI Model - Deep Dive](#osi-model---deep-dive)
3. [TCP/IP Model](#tcpip-model)
4. [IP Addressing & Subnetting](#ip-addressing--subnetting)
5. [Network Protocols](#network-protocols)
6. [DNS - Domain Name System](#dns---domain-name-system)
7. [HTTP/HTTPS & Web Protocols](#httphttps--web-protocols)
8. [Network Security](#network-security)
9. [Load Balancing & Proxies](#load-balancing--proxies)
10. [Network Troubleshooting](#network-troubleshooting)
11. [Cloud Networking](#cloud-networking)
12. [Interview Questions & Answers](#interview-questions--answers)

---

## Networking Fundamentals

### What is a Computer Network?
A computer network is a collection of interconnected devices that can communicate and share resources with each other.

### Types of Networks

| Type | Full Form | Range | Example |
|------|-----------|-------|---------|
| PAN | Personal Area Network | ~10 meters | Bluetooth devices |
| LAN | Local Area Network | Building/Campus | Office network |
| MAN | Metropolitan Area Network | City | Cable TV network |
| WAN | Wide Area Network | Global | Internet |

### Network Topologies

```
1. BUS TOPOLOGY
   ┌───┐   ┌───┐   ┌───┐   ┌───┐
   │PC1│   │PC2│   │PC3│   │PC4│
   └─┬─┘   └─┬─┘   └─┬─┘   └─┬─┘
     │       │       │       │
   ══╧═══════╧═══════╧═══════╧══  (Single cable/bus)

2. STAR TOPOLOGY
        ┌───┐
        │PC1│
        └─┬─┘
          │
   ┌───┐  │  ┌───┐
   │PC2├──┼──┤PC3│
   └───┘  │  └───┘
       ┌──┴──┐
       │ HUB │
       └──┬──┘
          │
        ┌─┴─┐
        │PC4│
        └───┘

3. RING TOPOLOGY
       ┌───┐
       │PC1│
       └─┬─┘
      ╱     ╲
   ┌─┴─┐   ┌─┴─┐
   │PC4│   │PC2│
   └─┬─┘   └─┬─┘
      ╲     ╱
       └─┬─┘
       ┌─┴─┐
       │PC3│
       └───┘

4. MESH TOPOLOGY
   ┌───┐─────────┌───┐
   │PC1│╲       ╱│PC2│
   └─┬─┘ ╲     ╱ └─┬─┘
     │    ╲   ╱    │
     │     ╲ ╱     │
     │      ╳      │
     │     ╱ ╲     │
     │    ╱   ╲    │
   ┌─┴─┐ ╱     ╲ ┌─┴─┐
   │PC4│╱───────╲│PC3│
   └───┘         └───┘
```

### Network Devices

| Device | Layer | Function |
|--------|-------|----------|
| Hub | Physical (1) | Broadcasts data to all ports |
| Repeater | Physical (1) | Amplifies/regenerates signal |
| Bridge | Data Link (2) | Connects two LANs, filters by MAC |
| Switch | Data Link (2) | Intelligent forwarding by MAC address |
| Router | Network (3) | Routes packets between networks |
| Gateway | All Layers | Protocol converter between networks |
| Firewall | Multiple | Security - filters network traffic |

---

## OSI Model - Deep Dive

### Overview

The **OSI (Open Systems Interconnection)** model is a conceptual framework that standardizes network communication into 7 layers.

```
┌─────────────────────────────────────────────────────────────────┐
│                    OSI MODEL - 7 LAYERS                         │
├─────────────────────────────────────────────────────────────────┤
│  Layer 7 │ APPLICATION  │ HTTP, FTP, SMTP, DNS    │ Data       │
├──────────┼──────────────┼─────────────────────────┼────────────┤
│  Layer 6 │ PRESENTATION │ SSL/TLS, JPEG, ASCII    │ Data       │
├──────────┼──────────────┼─────────────────────────┼────────────┤
│  Layer 5 │ SESSION      │ NetBIOS, RPC, SQL       │ Data       │
├──────────┼──────────────┼─────────────────────────┼────────────┤
│  Layer 4 │ TRANSPORT    │ TCP, UDP                │ Segments   │
├──────────┼──────────────┼─────────────────────────┼────────────┤
│  Layer 3 │ NETWORK      │ IP, ICMP, ARP, OSPF     │ Packets    │
├──────────┼──────────────┼─────────────────────────┼────────────┤
│  Layer 2 │ DATA LINK    │ Ethernet, PPP, Switch   │ Frames     │
├──────────┼──────────────┼─────────────────────────┼────────────┤
│  Layer 1 │ PHYSICAL     │ Cables, Hubs, Signals   │ Bits       │
└─────────────────────────────────────────────────────────────────┘

Mnemonic: "All People Seem To Need Data Processing"
          (Application → Physical)

Reverse:  "Please Do Not Throw Sausage Pizza Away"
          (Physical → Application)
```

### Layer 1: Physical Layer

**Purpose**: Transmits raw bits over a physical medium

**Characteristics**:
- Defines physical specifications (cables, connectors, voltages)
- Deals with bit synchronization
- Defines transmission mode (simplex, half-duplex, full-duplex)

**Components**:
- Cables (Coaxial, Fiber Optic, Twisted Pair)
- Hubs, Repeaters
- Network Interface Cards (NIC)
- Connectors (RJ-45, BNC)

```
Transmission Modes:
┌─────────────────────────────────────────────────────────┐
│ SIMPLEX        │  A ──────────────► B                  │
│ (One-way)      │  (Keyboard → Computer)                │
├────────────────┼────────────────────────────────────────┤
│ HALF-DUPLEX    │  A ◄─────────────► B                  │
│ (Two-way,      │  (Walkie-Talkie)                      │
│  alternating)  │                                        │
├────────────────┼────────────────────────────────────────┤
│ FULL-DUPLEX    │  A ◄══════════════► B                 │
│ (Two-way,      │  (Telephone)                          │
│  simultaneous) │                                        │
└────────────────┴────────────────────────────────────────┘
```

**Cable Types**:

| Cable Type | Speed | Distance | Use Case |
|------------|-------|----------|----------|
| Cat5e | 1 Gbps | 100m | Basic networking |
| Cat6 | 10 Gbps | 55m | High-speed LAN |
| Cat6a | 10 Gbps | 100m | Data centers |
| Cat7 | 10 Gbps | 100m | Enterprise |
| Fiber Optic (Single Mode) | 100+ Gbps | 100+ km | Long distance |
| Fiber Optic (Multi Mode) | 10 Gbps | 2 km | Campus/Building |

### Layer 2: Data Link Layer

**Purpose**: Node-to-node data transfer, error detection/correction

**Sub-layers**:
1. **LLC (Logical Link Control)**: Flow control, error checking
2. **MAC (Media Access Control)**: Physical addressing

**Key Concepts**:

```
MAC Address Structure (48 bits / 6 bytes):
┌─────────────────────────────────────────────┐
│  AA : BB : CC : DD : EE : FF               │
│  └──────┬──────┘   └──────┬──────┘         │
│    OUI (Vendor)    Device Identifier        │
│    (24 bits)       (24 bits)                │
└─────────────────────────────────────────────┘

Example: 00:1A:2B:3C:4D:5E
         └─Intel─┘
```

**Frame Structure**:
```
┌──────────┬──────────┬──────────┬─────────┬──────────┬─────┐
│ Preamble │ Dest MAC │ Src MAC  │  Type   │  Data    │ FCS │
│ (8 bytes)│ (6 bytes)│ (6 bytes)│(2 bytes)│(46-1500) │(4 B)│
└──────────┴──────────┴──────────┴─────────┴──────────┴─────┘
```

**Switching Methods**:
- **Store-and-Forward**: Receives entire frame, checks CRC, then forwards
- **Cut-Through**: Forwards immediately after reading destination MAC
- **Fragment-Free**: Reads first 64 bytes before forwarding

**VLAN (Virtual LAN)**:
```
Without VLAN:                    With VLAN:
┌─────────────────┐              ┌─────────────────┐
│     SWITCH      │              │     SWITCH      │
│  ┌──┬──┬──┬──┐  │              │  ┌──┬──┬──┬──┐  │
│  │1 │2 │3 │4 │  │              │  │1 │2 │3 │4 │  │
└──┴──┴──┴──┴──┴──┘              └──┴──┴──┴──┴──┴──┘
   │  │  │  │                       │  │  │  │
   All in same                    VLAN1  VLAN2
   broadcast domain               │  │    │  │
                                 HR Dept  IT Dept
```

### Layer 3: Network Layer

**Purpose**: Logical addressing, routing, packet forwarding

**Key Protocols**:
- **IP (Internet Protocol)**: Logical addressing
- **ICMP (Internet Control Message Protocol)**: Error reporting (ping)
- **ARP (Address Resolution Protocol)**: IP to MAC mapping
- **RARP (Reverse ARP)**: MAC to IP mapping

**IP Packet Structure**:
```
┌─────────────────────────────────────────────────────────────┐
│ Version │ IHL │ TOS │      Total Length (16 bits)          │
├─────────┴─────┴─────┼───────────────────┬──────────────────┤
│   Identification    │ Flags │  Fragment Offset             │
├─────────────────────┼───────┴──────────────────────────────┤
│  TTL    │ Protocol  │        Header Checksum               │
├─────────┴───────────┴──────────────────────────────────────┤
│                Source IP Address (32 bits)                  │
├────────────────────────────────────────────────────────────┤
│              Destination IP Address (32 bits)               │
├────────────────────────────────────────────────────────────┤
│                    Options (if any)                         │
├────────────────────────────────────────────────────────────┤
│                         DATA                                │
└────────────────────────────────────────────────────────────┘
```

**Routing Protocols**:

| Protocol | Type | Algorithm | Metric |
|----------|------|-----------|--------|
| RIP | Distance Vector | Bellman-Ford | Hop Count (max 15) |
| OSPF | Link State | Dijkstra | Cost (bandwidth) |
| EIGRP | Hybrid | DUAL | Composite metric |
| BGP | Path Vector | Best Path | AS Path, Policy |

**ARP Process**:
```
1. Host A wants to send to Host B (knows IP, needs MAC)

   Host A                                    Host B
   ┌─────┐                                   ┌─────┐
   │     │  ARP Request (Broadcast)          │     │
   │ IP: │  "Who has 192.168.1.2?"           │ IP: │
   │.1.1 │ ─────────────────────────────────►│.1.2 │
   │     │                                   │     │
   │     │  ARP Reply (Unicast)              │     │
   │     │  "192.168.1.2 is at AA:BB:CC..."  │     │
   │     │ ◄─────────────────────────────────│     │
   └─────┘                                   └─────┘

2. Host A caches the MAC in ARP table
3. Communication proceeds using MAC address
```

### Layer 4: Transport Layer

**Purpose**: End-to-end delivery, flow control, error recovery

**TCP vs UDP Comparison**:

| Feature | TCP | UDP |
|---------|-----|-----|
| Connection | Connection-oriented | Connectionless |
| Reliability | Guaranteed delivery | Best effort |
| Ordering | Maintains order | No ordering |
| Error Checking | Yes + Recovery | Checksum only |
| Flow Control | Yes (Sliding Window) | No |
| Speed | Slower | Faster |
| Header Size | 20-60 bytes | 8 bytes |
| Use Cases | HTTP, FTP, SMTP, SSH | DNS, DHCP, VoIP, Gaming |

**TCP Three-Way Handshake**:
```
    Client                              Server
      │                                    │
      │         SYN (seq=x)                │
      │ ──────────────────────────────────►│
      │                                    │
      │     SYN-ACK (seq=y, ack=x+1)       │
      │ ◄──────────────────────────────────│
      │                                    │
      │         ACK (ack=y+1)              │
      │ ──────────────────────────────────►│
      │                                    │
      │      Connection Established        │
      │ ◄─────────────────────────────────►│
```

**TCP Four-Way Termination**:
```
    Client                              Server
      │                                    │
      │         FIN (seq=x)                │
      │ ──────────────────────────────────►│
      │                                    │
      │         ACK (ack=x+1)              │
      │ ◄──────────────────────────────────│
      │                                    │
      │         FIN (seq=y)                │
      │ ◄──────────────────────────────────│
      │                                    │
      │         ACK (ack=y+1)              │
      │ ──────────────────────────────────►│
      │                                    │
      │      Connection Terminated         │
```

**TCP Header Structure**:
```
┌─────────────────────────┬─────────────────────────┐
│   Source Port (16)      │  Destination Port (16)  │
├─────────────────────────┴─────────────────────────┤
│              Sequence Number (32 bits)            │
├───────────────────────────────────────────────────┤
│           Acknowledgment Number (32 bits)         │
├────────┬────────┬─────────────────────────────────┤
│Offset  │Reserved│ Flags   │    Window (16)        │
│  (4)   │  (6)   │URG|ACK|PSH|RST|SYN|FIN         │
├────────┴────────┴─────────┬───────────────────────┤
│     Checksum (16)         │  Urgent Pointer (16)  │
├───────────────────────────┴───────────────────────┤
│                Options (if any)                   │
└───────────────────────────────────────────────────┘
```

**Common Port Numbers**:

| Port | Protocol | Service |
|------|----------|---------|
| 20/21 | TCP | FTP (Data/Control) |
| 22 | TCP | SSH |
| 23 | TCP | Telnet |
| 25 | TCP | SMTP |
| 53 | TCP/UDP | DNS |
| 67/68 | UDP | DHCP |
| 80 | TCP | HTTP |
| 110 | TCP | POP3 |
| 143 | TCP | IMAP |
| 443 | TCP | HTTPS |
| 3306 | TCP | MySQL |
| 3389 | TCP | RDP |
| 5432 | TCP | PostgreSQL |

### Layer 5: Session Layer

**Purpose**: Establishes, manages, and terminates sessions

**Functions**:
- Session establishment, maintenance, termination
- Synchronization (checkpoints)
- Dialog control (half-duplex, full-duplex)

**Protocols**:
- NetBIOS
- RPC (Remote Procedure Call)
- SQL
- NFS (Network File System)

```
Session Management:
┌────────────────────────────────────────────────────────┐
│                    SESSION LAYER                        │
├────────────────────────────────────────────────────────┤
│  1. Session Establishment                               │
│     ┌──────┐  "Let's talk"   ┌──────┐                  │
│     │App A │ ───────────────►│App B │                  │
│     └──────┘ ◄───────────────└──────┘                  │
│              "OK, let's talk"                          │
├────────────────────────────────────────────────────────┤
│  2. Data Transfer with Checkpoints                     │
│     [Data] → [Checkpoint 1] → [Data] → [Checkpoint 2]  │
│     (If failure, resume from last checkpoint)          │
├────────────────────────────────────────────────────────┤
│  3. Session Termination                                │
│     ┌──────┐  "Goodbye"      ┌──────┐                  │
│     │App A │ ───────────────►│App B │                  │
│     └──────┘                 └──────┘                  │
└────────────────────────────────────────────────────────┘
```

### Layer 6: Presentation Layer

**Purpose**: Data translation, encryption, compression

**Functions**:
- **Translation**: ASCII ↔ EBCDIC, character encoding
- **Encryption/Decryption**: SSL/TLS
- **Compression**: JPEG, MPEG, GIF

```
Data Transformation Pipeline:
┌─────────────────────────────────────────────────────┐
│  Application Data                                    │
│        │                                             │
│        ▼                                             │
│  ┌─────────────┐                                     │
│  │ Translation │  (Character encoding)               │
│  └──────┬──────┘                                     │
│         ▼                                            │
│  ┌─────────────┐                                     │
│  │ Encryption  │  (SSL/TLS)                          │
│  └──────┬──────┘                                     │
│         ▼                                            │
│  ┌─────────────┐                                     │
│  │ Compression │  (Reduce size)                      │
│  └──────┬──────┘                                     │
│         ▼                                            │
│  Formatted Data → Session Layer                      │
└─────────────────────────────────────────────────────┘
```

### Layer 7: Application Layer

**Purpose**: Interface between user and network

**Common Protocols**:

| Protocol | Port | Description |
|----------|------|-------------|
| HTTP | 80 | Web browsing |
| HTTPS | 443 | Secure web browsing |
| FTP | 20/21 | File transfer |
| SMTP | 25 | Email sending |
| POP3 | 110 | Email retrieval |
| IMAP | 143 | Email access |
| DNS | 53 | Domain name resolution |
| DHCP | 67/68 | IP address assignment |
| SNMP | 161/162 | Network management |
| SSH | 22 | Secure shell |
| Telnet | 23 | Remote access (insecure) |

---

## TCP/IP Model

### OSI vs TCP/IP Comparison

```
┌─────────────────────┐     ┌─────────────────────┐
│      OSI Model      │     │    TCP/IP Model     │
├─────────────────────┤     ├─────────────────────┤
│  7. Application     │     │                     │
├─────────────────────┤     │    Application      │
│  6. Presentation    │     │    (HTTP, FTP,      │
├─────────────────────┤     │     SMTP, DNS)      │
│  5. Session         │     │                     │
├─────────────────────┤     ├─────────────────────┤
│  4. Transport       │     │    Transport        │
│     (TCP, UDP)      │     │    (TCP, UDP)       │
├─────────────────────┤     ├─────────────────────┤
│  3. Network         │     │    Internet         │
│     (IP, ICMP)      │     │    (IP, ICMP, ARP)  │
├─────────────────────┤     ├─────────────────────┤
│  2. Data Link       │     │                     │
├─────────────────────┤     │    Network Access   │
│  1. Physical        │     │    (Ethernet, Wi-Fi)│
└─────────────────────┘     └─────────────────────┘
```

### TCP/IP Encapsulation

```
┌────────────────────────────────────────────────────────────┐
│ Application Layer │         DATA                           │
├───────────────────┼────────────────────────────────────────┤
│ Transport Layer   │ TCP/UDP │       DATA                   │
│                   │ Header  │                              │
├───────────────────┼─────────┼──────────────────────────────┤
│ Internet Layer    │   IP    │ TCP/UDP │      DATA          │
│                   │ Header  │ Header  │                    │
├───────────────────┼─────────┼─────────┼────────────────────┤
│ Network Access    │Ethernet │   IP    │ TCP/UDP │   DATA   │
│                   │ Header  │ Header  │ Header  │          │
└───────────────────┴─────────┴─────────┴─────────┴──────────┘
```

---

## IP Addressing & Subnetting

### IPv4 Address Structure

```
IPv4 Address: 32 bits divided into 4 octets
┌─────────┬─────────┬─────────┬─────────┐
│  8 bits │  8 bits │  8 bits │  8 bits │
├─────────┼─────────┼─────────┼─────────┤
│   192   │   168   │    1    │   100   │
├─────────┴─────────┼─────────┴─────────┤
│    Network ID     │      Host ID      │
└───────────────────┴───────────────────┘
```

### IP Address Classes

| Class | Range | Default Mask | Network Bits | Host Bits | Networks | Hosts/Network |
|-------|-------|--------------|--------------|-----------|----------|---------------|
| A | 1.0.0.0 - 126.255.255.255 | 255.0.0.0 | 8 | 24 | 126 | 16,777,214 |
| B | 128.0.0.0 - 191.255.255.255 | 255.255.0.0 | 16 | 16 | 16,384 | 65,534 |
| C | 192.0.0.0 - 223.255.255.255 | 255.255.255.0 | 24 | 8 | 2,097,152 | 254 |
| D | 224.0.0.0 - 239.255.255.255 | - | - | - | Multicast | - |
| E | 240.0.0.0 - 255.255.255.255 | - | - | - | Reserved | - |

### Private IP Ranges (RFC 1918)

| Class | Range | CIDR |
|-------|-------|------|
| A | 10.0.0.0 - 10.255.255.255 | 10.0.0.0/8 |
| B | 172.16.0.0 - 172.31.255.255 | 172.16.0.0/12 |
| C | 192.168.0.0 - 192.168.255.255 | 192.168.0.0/16 |

### Special IP Addresses

| Address | Purpose |
|---------|---------|
| 0.0.0.0 | Default route / "This host" |
| 127.0.0.1 | Loopback (localhost) |
| 255.255.255.255 | Broadcast (all hosts) |
| 169.254.x.x | APIPA (Auto-assigned when DHCP fails) |

### Subnetting

**Subnet Mask**: Separates network portion from host portion

```
IP Address:    192.168.1.100
Subnet Mask:   255.255.255.0

Binary Representation:
IP:    11000000.10101000.00000001.01100100
Mask:  11111111.11111111.11111111.00000000
       └────────Network────────┘└──Host──┘
```

**CIDR Notation**:

| CIDR | Subnet Mask | Hosts | Usable Hosts |
|------|-------------|-------|--------------|
| /24 | 255.255.255.0 | 256 | 254 |
| /25 | 255.255.255.128 | 128 | 126 |
| /26 | 255.255.255.192 | 64 | 62 |
| /27 | 255.255.255.224 | 32 | 30 |
| /28 | 255.255.255.240 | 16 | 14 |
| /29 | 255.255.255.248 | 8 | 6 |
| /30 | 255.255.255.252 | 4 | 2 |
| /31 | 255.255.255.254 | 2 | 2 (Point-to-point) |
| /32 | 255.255.255.255 | 1 | 1 (Single host) |

**Subnetting Example**:
```
Given: 192.168.10.0/24
Requirement: Create 4 subnets

Step 1: Calculate bits needed
        4 subnets = 2^2, need 2 bits

Step 2: New subnet mask
        /24 + 2 = /26 (255.255.255.192)

Step 3: Calculate subnets
        Block size = 256 - 192 = 64

Subnet 1: 192.168.10.0/26
          Network:   192.168.10.0
          First:     192.168.10.1
          Last:      192.168.10.62
          Broadcast: 192.168.10.63

Subnet 2: 192.168.10.64/26
          Network:   192.168.10.64
          First:     192.168.10.65
          Last:      192.168.10.126
          Broadcast: 192.168.10.127

Subnet 3: 192.168.10.128/26
          Network:   192.168.10.128
          First:     192.168.10.129
          Last:      192.168.10.190
          Broadcast: 192.168.10.191

Subnet 4: 192.168.10.192/26
          Network:   192.168.10.192
          First:     192.168.10.193
          Last:      192.168.10.254
          Broadcast: 192.168.10.255
```

### IPv6 Addressing

**Structure**: 128 bits, 8 groups of 4 hex digits

```
Full:     2001:0db8:85a3:0000:0000:8a2e:0370:7334
Shortened: 2001:db8:85a3::8a2e:370:7334

Rules:
1. Leading zeros in group can be removed
2. Consecutive zero groups → :: (only once)
```

**IPv6 Address Types**:

| Type | Prefix | Description |
|------|--------|-------------|
| Global Unicast | 2000::/3 | Public addresses |
| Link-Local | fe80::/10 | Auto-configured, local link |
| Unique Local | fc00::/7 | Private addresses |
| Multicast | ff00::/8 | One-to-many |
| Loopback | ::1 | Localhost |

---

## Network Protocols

### DHCP (Dynamic Host Configuration Protocol)

**Purpose**: Automatically assigns IP addresses and network configuration

```
DHCP DORA Process:
┌────────────────────────────────────────────────────────────┐
│  Client                              DHCP Server           │
│    │                                      │                │
│    │  1. DISCOVER (Broadcast)             │                │
│    │  "I need an IP address"              │                │
│    │ ────────────────────────────────────►│                │
│    │                                      │                │
│    │  2. OFFER (Unicast/Broadcast)        │                │
│    │  "Here's 192.168.1.100"              │                │
│    │ ◄────────────────────────────────────│                │
│    │                                      │                │
│    │  3. REQUEST (Broadcast)              │                │
│    │  "I'll take 192.168.1.100"           │                │
│    │ ────────────────────────────────────►│                │
│    │                                      │                │
│    │  4. ACK (Unicast)                    │                │
│    │  "Confirmed, it's yours"             │                │
│    │ ◄────────────────────────────────────│                │
└────────────────────────────────────────────────────────────┘
```

**DHCP Lease Information Includes**:
- IP Address
- Subnet Mask
- Default Gateway
- DNS Servers
- Lease Duration
- Domain Name

### NAT (Network Address Translation)

**Purpose**: Translates private IP addresses to public IP addresses

**Types of NAT**:

```
1. STATIC NAT (1:1 mapping)
   ┌──────────────────┐      ┌──────────────────┐
   │ Private: 10.0.0.1│ ───► │ Public: 203.0.1.1│
   │ Private: 10.0.0.2│ ───► │ Public: 203.0.1.2│
   └──────────────────┘      └──────────────────┘

2. DYNAMIC NAT (Pool-based)
   ┌──────────────────┐      ┌──────────────────┐
   │ 10.0.0.1 ────────┼─┐    │ Pool:            │
   │ 10.0.0.2 ────────┼─┼───►│ 203.0.1.1-1.10   │
   │ 10.0.0.3 ────────┼─┘    │ (First available)│
   └──────────────────┘      └──────────────────┘

3. PAT / NAPT (Port Address Translation)
   ┌──────────────────┐      ┌──────────────────┐
   │ 10.0.0.1:5000 ───┼─┐    │ 203.0.1.1:10001  │
   │ 10.0.0.2:5000 ───┼─┼───►│ 203.0.1.1:10002  │
   │ 10.0.0.3:5000 ───┼─┘    │ 203.0.1.1:10003  │
   └──────────────────┘      └──────────────────┘
   (Many-to-one using different ports)
```

### ICMP (Internet Control Message Protocol)

**Purpose**: Network diagnostics and error reporting

**Common ICMP Types**:

| Type | Code | Description |
|------|------|-------------|
| 0 | 0 | Echo Reply (ping response) |
| 3 | 0 | Destination Network Unreachable |
| 3 | 1 | Destination Host Unreachable |
| 3 | 3 | Destination Port Unreachable |
| 5 | 0 | Redirect (better route available) |
| 8 | 0 | Echo Request (ping) |
| 11 | 0 | TTL Exceeded (traceroute) |

```
Ping Process:
┌────────┐  ICMP Echo Request (Type 8)   ┌────────┐
│ Host A │ ─────────────────────────────►│ Host B │
│        │ ◄─────────────────────────────│        │
└────────┘  ICMP Echo Reply (Type 0)     └────────┘

Traceroute Process:
┌────────┐                               ┌────────┐
│ Source │──► Router1 ──► Router2 ──────►│ Dest   │
└────────┘     │            │            └────────┘
               │            │
         TTL=1 │      TTL=2 │
         Reply │      Reply │
         "I'm  │      "I'm  │
         here" │      here" │
```

---

## DNS - Domain Name System

### DNS Hierarchy

```
                         ┌──────┐
                         │  .   │  (Root)
                         └──┬───┘
          ┌─────────────────┼─────────────────┐
          ▼                 ▼                 ▼
       ┌─────┐          ┌─────┐          ┌─────┐
       │.com │          │.org │          │.net │  (TLD)
       └──┬──┘          └─────┘          └─────┘
          │
    ┌─────┴─────┐
    ▼           ▼
┌───────┐  ┌───────┐
│google │  │amazon │  (Second Level)
└───┬───┘  └───────┘
    │
    ▼
┌──────┐
│ www  │  (Subdomain)
└──────┘

FQDN: www.google.com.
```

### DNS Record Types

| Record | Purpose | Example |
|--------|---------|---------|
| A | IPv4 address | example.com → 93.184.216.34 |
| AAAA | IPv6 address | example.com → 2606:2800:220:1:... |
| CNAME | Canonical name (alias) | www → example.com |
| MX | Mail exchanger | example.com → mail.example.com |
| NS | Name server | example.com → ns1.example.com |
| TXT | Text record | SPF, DKIM, verification |
| PTR | Reverse lookup | IP → domain name |
| SOA | Start of Authority | Zone information |
| SRV | Service record | _sip._tcp.example.com |

### DNS Resolution Process

```
┌──────────────────────────────────────────────────────────────────┐
│                    DNS Resolution for www.example.com            │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌────────┐      ┌────────────┐                                  │
│  │ Client │──1──►│ Recursive  │                                  │
│  │        │◄─8───│  Resolver  │                                  │
│  └────────┘      └─────┬──────┘                                  │
│                        │                                         │
│                   2    │    ┌─────────────────┐                  │
│                   ┌────┴───►│  Root Server    │                  │
│                   │    ◄─3──│  (Returns .com) │                  │
│                   │         └─────────────────┘                  │
│                   │                                              │
│                   │    4    ┌─────────────────┐                  │
│                   ├────────►│  .com TLD       │                  │
│                   │    ◄─5──│  (Returns NS)   │                  │
│                   │         └─────────────────┘                  │
│                   │                                              │
│                   │    6    ┌─────────────────┐                  │
│                   └────────►│  example.com NS │                  │
│                        ◄─7──│  (Returns IP)   │                  │
│                             └─────────────────┘                  │
│                                                                  │
│  Steps:                                                          │
│  1. Client queries recursive resolver                            │
│  2-3. Resolver asks root server → gets .com TLD address          │
│  4-5. Resolver asks .com TLD → gets example.com NS address       │
│  6-7. Resolver asks authoritative NS → gets IP address           │
│  8. Resolver returns IP to client (caches result)                │
│                                                                  │
└──────────────────────────────────────────────────────────────────┘
```

### DNS Caching & TTL

```
DNS Cache Hierarchy:
┌─────────────────────────────────────────────┐
│  1. Browser Cache (Chrome, Firefox)         │
│     ↓ (if not found)                        │
│  2. Operating System Cache                  │
│     ↓ (if not found)                        │
│  3. Router Cache                            │
│     ↓ (if not found)                        │
│  4. ISP Recursive Resolver Cache            │
│     ↓ (if not found)                        │
│  5. Full DNS Resolution                     │
└─────────────────────────────────────────────┘

TTL (Time To Live):
- Determines how long DNS record is cached
- Lower TTL = More frequent lookups, faster propagation
- Higher TTL = Less DNS traffic, slower propagation
- Typical values: 300s (5min) to 86400s (24hr)
```

---

## HTTP/HTTPS & Web Protocols

### HTTP Methods

| Method | Purpose | Idempotent | Safe |
|--------|---------|------------|------|
| GET | Retrieve resource | Yes | Yes |
| POST | Create resource | No | No |
| PUT | Update/Replace resource | Yes | No |
| PATCH | Partial update | No | No |
| DELETE | Remove resource | Yes | No |
| HEAD | GET without body | Yes | Yes |
| OPTIONS | Get allowed methods | Yes | Yes |

### HTTP Status Codes

```
┌─────────────────────────────────────────────────────────────┐
│  1xx - Informational                                        │
│  100 Continue                                               │
│  101 Switching Protocols (WebSocket upgrade)                │
├─────────────────────────────────────────────────────────────┤
│  2xx - Success                                              │
│  200 OK                                                     │
│  201 Created                                                │
│  204 No Content                                             │
├─────────────────────────────────────────────────────────────┤
│  3xx - Redirection                                          │
│  301 Moved Permanently                                      │
│  302 Found (Temporary Redirect)                             │
│  304 Not Modified (Use cached version)                      │
├─────────────────────────────────────────────────────────────┤
│  4xx - Client Error                                         │
│  400 Bad Request                                            │
│  401 Unauthorized (Authentication required)                 │
│  403 Forbidden (Authenticated but not authorized)           │
│  404 Not Found                                              │
│  405 Method Not Allowed                                     │
│  429 Too Many Requests (Rate limiting)                      │
├─────────────────────────────────────────────────────────────┤
│  5xx - Server Error                                         │
│  500 Internal Server Error                                  │
│  502 Bad Gateway                                            │
│  503 Service Unavailable                                    │
│  504 Gateway Timeout                                        │
└─────────────────────────────────────────────────────────────┘
```

### HTTP vs HTTPS

```
HTTP (Insecure):
┌────────┐                              ┌────────┐
│ Client │ ══════════════════════════► │ Server │
└────────┘    Plain Text Data           └────────┘
              (Can be intercepted)

HTTPS (Secure):
┌────────┐  TLS Handshake  ┌────────┐
│ Client │ ◄─────────────► │ Server │
└────────┘                 └────────┘
     │                          │
     │   Encrypted Channel      │
     │ ════════════════════════►│
     │ ◄════════════════════════│
     │   (SSL/TLS Protected)    │
```

### TLS/SSL Handshake

```
┌────────────────────────────────────────────────────────────────┐
│                    TLS 1.2 Handshake                           │
├────────────────────────────────────────────────────────────────┤
│  Client                                Server                  │
│    │                                      │                    │
│    │  1. ClientHello                      │                    │
│    │  (Supported ciphers, TLS version)    │                    │
│    │ ────────────────────────────────────►│                    │
│    │                                      │                    │
│    │  2. ServerHello                      │                    │
│    │  (Selected cipher, certificate)      │                    │
│    │ ◄────────────────────────────────────│                    │
│    │                                      │                    │
│    │  3. Client verifies certificate      │                    │
│    │     Generates pre-master secret      │                    │
│    │     Encrypts with server's public key│                    │
│    │ ────────────────────────────────────►│                    │
│    │                                      │                    │
│    │  4. Both derive session keys         │                    │
│    │     Exchange "Finished" messages     │                    │
│    │ ◄───────────────────────────────────►│                    │
│    │                                      │                    │
│    │  5. Secure communication begins      │                    │
│    │ ════════════════════════════════════►│                    │
└────────────────────────────────────────────────────────────────┘
```

### HTTP/2 and HTTP/3

```
HTTP/1.1 vs HTTP/2 vs HTTP/3:

HTTP/1.1:
┌───────────────────────────────────────────┐
│  Request 1 ────────► Response 1           │
│  Request 2 ────────► Response 2           │
│  Request 3 ────────► Response 3           │
│  (Sequential, Head-of-line blocking)      │
└───────────────────────────────────────────┘

HTTP/2:
┌───────────────────────────────────────────┐
│  ══════ Single TCP Connection ══════      │
│  Stream 1: Request ──► Response           │
│  Stream 2: Request ──► Response           │
│  Stream 3: Request ──► Response           │
│  (Multiplexed, Binary, Server Push)       │
└───────────────────────────────────────────┘

HTTP/3:
┌───────────────────────────────────────────┐
│  ══════ QUIC over UDP ══════              │
│  Stream 1: Request ──► Response           │
│  Stream 2: Request ──► Response           │
│  (No TCP HOL blocking, 0-RTT resume)      │
└───────────────────────────────────────────┘
```

### WebSockets

```
WebSocket Connection:
┌────────────────────────────────────────────────────────────┐
│  1. HTTP Upgrade Request                                   │
│     GET /chat HTTP/1.1                                     │
│     Upgrade: websocket                                     │
│     Connection: Upgrade                                    │
│                                                            │
│  2. Server Response                                        │
│     HTTP/1.1 101 Switching Protocols                       │
│     Upgrade: websocket                                     │
│                                                            │
│  3. Full-duplex Communication                              │
│     ┌────────┐ ◄═══════════════════► ┌────────┐           │
│     │ Client │   Bidirectional       │ Server │           │
│     └────────┘   Real-time           └────────┘           │
└────────────────────────────────────────────────────────────┘

Use Cases:
- Real-time chat
- Live notifications
- Online gaming
- Stock tickers
- Collaborative editing
```

---

## Network Security

### Firewall Types

```
1. PACKET FILTERING FIREWALL (Layer 3-4)
   ┌─────────────────────────────────────────┐
   │  Rules based on:                        │
   │  - Source/Destination IP                │
   │  - Source/Destination Port              │
   │  - Protocol (TCP/UDP)                   │
   │                                         │
   │  Example: Block port 23 (Telnet)        │
   │           Allow port 443 (HTTPS)        │
   └─────────────────────────────────────────┘

2. STATEFUL INSPECTION FIREWALL (Layer 3-4)
   ┌─────────────────────────────────────────┐
   │  Tracks connection state                │
   │  Maintains state table                  │
   │                                         │
   │  States: NEW, ESTABLISHED, RELATED      │
   │                                         │
   │  More secure than packet filtering      │
   └─────────────────────────────────────────┘

3. APPLICATION LAYER FIREWALL / WAF (Layer 7)
   ┌─────────────────────────────────────────┐
   │  Inspects application data              │
   │  - SQL Injection detection              │
   │  - XSS prevention                       │
   │  - Protocol validation                  │
   │  - Content filtering                    │
   └─────────────────────────────────────────┘

4. NEXT-GEN FIREWALL (NGFW)
   ┌─────────────────────────────────────────┐
   │  Combines:                              │
   │  - Deep packet inspection               │
   │  - Intrusion prevention (IPS)           │
   │  - Application awareness                │
   │  - SSL/TLS inspection                   │
   │  - User identity integration            │
   └─────────────────────────────────────────┘
```

### VPN (Virtual Private Network)

```
VPN Tunnel:
┌──────────────────────────────────────────────────────────────┐
│                                                              │
│  ┌────────┐                              ┌────────────┐     │
│  │ Remote │═══════════════════════════════│ Corporate  │     │
│  │ User   │   Encrypted Tunnel            │ Network    │     │
│  └────────┘   over Internet               └────────────┘     │
│                                                              │
│  VPN Types:                                                  │
│  1. Site-to-Site: Connects two networks                      │
│  2. Remote Access: Connects user to network                  │
│  3. SSL VPN: Browser-based, no client needed                 │
│                                                              │
│  Protocols:                                                  │
│  - IPSec (Network layer)                                     │
│  - OpenVPN (SSL-based)                                       │
│  - WireGuard (Modern, fast)                                  │
│  - L2TP/IPSec (Layer 2)                                      │
└──────────────────────────────────────────────────────────────┘
```

### Common Network Attacks

```
1. MAN-IN-THE-MIDDLE (MITM)
   ┌────────┐      ┌──────────┐      ┌────────┐
   │ Client │ ────►│ Attacker │ ────►│ Server │
   └────────┘      └──────────┘      └────────┘
   Prevention: HTTPS, Certificate pinning

2. DNS SPOOFING
   Client asks for google.com
   Attacker responds with malicious IP
   Prevention: DNSSEC, DNS over HTTPS (DoH)

3. ARP SPOOFING
   Attacker sends fake ARP replies
   Traffic redirected to attacker
   Prevention: Static ARP entries, DAI

4. DDoS (Distributed Denial of Service)
   ┌────────┐
   │ Bot 1  │──┐
   └────────┘  │    ┌────────┐
   ┌────────┐  ├───►│ Target │ OVERWHELMED
   │ Bot 2  │──┤    └────────┘
   └────────┘  │
   ┌────────┐  │
   │ Bot N  │──┘
   └────────┘
   Prevention: Rate limiting, CDN, DDoS protection

5. SQL INJECTION / XSS
   Application layer attacks
   Prevention: Input validation, WAF, parameterized queries
```

### SSL/TLS Certificates

```
Certificate Chain:
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│  ┌───────────────────┐                                      │
│  │   Root CA Cert    │  (Pre-installed in browsers/OS)      │
│  │   (Self-signed)   │                                      │
│  └─────────┬─────────┘                                      │
│            │ Signs                                          │
│            ▼                                                │
│  ┌───────────────────┐                                      │
│  │ Intermediate CA   │  (Optional, adds security layer)     │
│  │     Cert          │                                      │
│  └─────────┬─────────┘                                      │
│            │ Signs                                          │
│            ▼                                                │
│  ┌───────────────────┐                                      │
│  │  Server Cert      │  (Your website's certificate)        │
│  │  (example.com)    │                                      │
│  └───────────────────┘                                      │
│                                                             │
│  Certificate Types:                                         │
│  - DV (Domain Validation): Basic, automated                 │
│  - OV (Organization Validation): Company verified           │
│  - EV (Extended Validation): Highest trust, green bar       │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## Load Balancing & Proxies

### Load Balancing Algorithms

```
┌─────────────────────────────────────────────────────────────────┐
│                    LOAD BALANCING ALGORITHMS                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  1. ROUND ROBIN                                                 │
│     Request 1 → Server A                                        │
│     Request 2 → Server B                                        │
│     Request 3 → Server C                                        │
│     Request 4 → Server A (cycle repeats)                        │
│                                                                 │
│  2. WEIGHTED ROUND ROBIN                                        │
│     Server A (weight 3): Gets 3x more traffic                   │
│     Server B (weight 1): Gets 1x traffic                        │
│                                                                 │
│  3. LEAST CONNECTIONS                                           │
│     Routes to server with fewest active connections             │
│     Good for varying request complexity                         │
│                                                                 │
│  4. IP HASH                                                     │
│     Hash(Client IP) → Always same server                        │
│     Provides session persistence                                │
│                                                                 │
│  5. LEAST RESPONSE TIME                                         │
│     Routes to fastest responding server                         │
│     Considers both connections and response time                │
│                                                                 │
│  6. RANDOM                                                      │
│     Randomly selects server                                     │
│     Simple but effective for homogeneous servers                │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Load Balancer Types

```
Layer 4 Load Balancer (Transport Layer):
┌────────────────────────────────────────────────────────────┐
│  - Works with TCP/UDP                                      │
│  - Routes based on IP and port                             │
│  - Cannot inspect HTTP content                             │
│  - Very fast, low latency                                  │
│  - Examples: AWS NLB, HAProxy (TCP mode)                   │
└────────────────────────────────────────────────────────────┘

Layer 7 Load Balancer (Application Layer):
┌────────────────────────────────────────────────────────────┐
│  - Works with HTTP/HTTPS                                   │
│  - Can route based on URL, headers, cookies                │
│  - SSL termination                                         │
│  - Content-based routing                                   │
│  - Examples: AWS ALB, Nginx, HAProxy (HTTP mode)           │
│                                                            │
│  Example routing:                                          │
│  /api/* → API servers                                      │
│  /static/* → CDN                                           │
│  /admin/* → Admin servers                                  │
└────────────────────────────────────────────────────────────┘
```

### Proxy Types

```
FORWARD PROXY:
┌────────┐     ┌───────┐     ┌────────┐
│ Client │ ───►│ Proxy │ ───►│ Server │
└────────┘     └───────┘     └────────┘
- Client knows about proxy
- Hides client identity
- Content filtering, caching
- Corporate network access control

REVERSE PROXY:
┌────────┐     ┌───────┐     ┌─────────────┐
│ Client │ ───►│ Proxy │ ───►│ Server Pool │
└────────┘     └───────┘     └─────────────┘
- Server-side proxy
- Client doesn't know about backend servers
- Load balancing, SSL termination
- Caching, security, compression

TRANSPARENT PROXY:
┌────────┐     ┌───────┐     ┌────────┐
│ Client │ ═══►│ Proxy │ ───►│ Server │
└────────┘     └───────┘     └────────┘
- Client unaware of proxy
- Traffic intercepted automatically
- ISP caching, content filtering
```

### CDN (Content Delivery Network)

```
Without CDN:
┌────────┐                    ┌────────────┐
│ User   │ ══════════════════►│   Origin   │
│ (Asia) │   High latency     │  (US East) │
└────────┘                    └────────────┘

With CDN:
┌────────┐     ┌────────────┐     ┌────────────┐
│ User   │ ───►│ Edge Server│ ···►│   Origin   │
│ (Asia) │     │   (Asia)   │     │  (US East) │
└────────┘     └────────────┘     └────────────┘
              Low latency          Cache miss only

Benefits:
- Reduced latency (geographic proximity)
- Reduced origin load
- DDoS protection
- SSL offloading
- Static content caching
```

---

## Network Troubleshooting

### Essential Commands

```bash
# DNS Lookup
nslookup example.com
dig example.com
host example.com

# Ping - Test connectivity
ping google.com
ping -c 4 google.com        # Linux: 4 packets
ping -n 4 google.com        # Windows: 4 packets

# Traceroute - Path to destination
traceroute google.com       # Linux
tracert google.com          # Windows

# Network Configuration
ifconfig                    # Linux (deprecated)
ip addr                     # Linux (modern)
ipconfig                    # Windows
ipconfig /all               # Windows detailed

# ARP Table
arp -a                      # View ARP cache

# Routing Table
route -n                    # Linux
route print                 # Windows
ip route                    # Linux (modern)

# Port Scanning / Connection Check
netstat -an                 # All connections
netstat -tulpn              # Linux: listening ports
ss -tulpn                   # Linux: modern replacement
netstat -b                  # Windows: with process names

# TCP Connection Test
telnet host port
nc -zv host port            # Netcat

# DNS Cache
ipconfig /flushdns          # Windows
systemd-resolve --flush-caches  # Linux systemd
sudo dscacheutil -flushcache    # macOS

# Packet Capture
tcpdump -i eth0
tcpdump -i eth0 port 80
wireshark                   # GUI tool
```

### Common Network Issues

```
┌─────────────────────────────────────────────────────────────────┐
│                    TROUBLESHOOTING FLOWCHART                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  Can't reach website?                                           │
│         │                                                       │
│         ▼                                                       │
│  ┌─────────────────┐                                            │
│  │ ping localhost  │ ──FAIL──► NIC/TCP stack issue              │
│  └────────┬────────┘                                            │
│           │ OK                                                  │
│           ▼                                                     │
│  ┌─────────────────┐                                            │
│  │ ping gateway    │ ──FAIL──► Local network issue              │
│  └────────┬────────┘           Check cable, switch, DHCP        │
│           │ OK                                                  │
│           ▼                                                     │
│  ┌─────────────────┐                                            │
│  │ ping 8.8.8.8    │ ──FAIL──► ISP/routing issue                │
│  └────────┬────────┘           Check router, ISP                │
│           │ OK                                                  │
│           ▼                                                     │
│  ┌─────────────────┐                                            │
│  │ nslookup domain │ ──FAIL──► DNS issue                        │
│  └────────┬────────┘           Try different DNS server         │
│           │ OK                                                  │
│           ▼                                                     │
│  ┌─────────────────┐                                            │
│  │ curl/wget site  │ ──FAIL──► Application/firewall issue       │
│  └────────┬────────┘           Check ports, firewall rules      │
│           │ OK                                                  │
│           ▼                                                     │
│  Issue resolved or browser-specific                             │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Wireshark Filters

```
# Common Display Filters
ip.addr == 192.168.1.1         # Filter by IP
tcp.port == 80                  # Filter by port
http                            # HTTP traffic only
dns                             # DNS traffic only
tcp.flags.syn == 1              # SYN packets
tcp.analysis.retransmission     # Retransmissions
!(arp or icmp or dns)           # Exclude common protocols

# Capture Filters
host 192.168.1.1                # Traffic to/from IP
port 443                        # HTTPS traffic
tcp port 80                     # HTTP traffic
not broadcast                   # Exclude broadcasts
```

---

## Cloud Networking

### AWS Networking

```
┌─────────────────────────────────────────────────────────────────┐
│                         AWS VPC                                 │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  VPC (Virtual Private Cloud): 10.0.0.0/16                       │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │                                                         │   │
│  │  ┌─────────────────────┐  ┌─────────────────────┐       │   │
│  │  │  Public Subnet      │  │  Private Subnet     │       │   │
│  │  │  10.0.1.0/24        │  │  10.0.2.0/24        │       │   │
│  │  │                     │  │                     │       │   │
│  │  │  ┌─────┐  ┌─────┐   │  │  ┌─────┐  ┌─────┐   │       │   │
│  │  │  │ EC2 │  │ EC2 │   │  │  │ RDS │  │ EC2 │   │       │   │
│  │  │  └─────┘  └─────┘   │  │  └─────┘  └─────┘   │       │   │
│  │  │       │             │  │       ▲             │       │   │
│  │  └───────┼─────────────┘  └───────┼─────────────┘       │   │
│  │          │                        │                     │   │
│  │          │        NAT Gateway     │                     │   │
│  │          │      ┌─────────────┐   │                     │   │
│  │          │      │     NAT     │───┘                     │   │
│  │          │      └─────────────┘                         │   │
│  │          │                                              │   │
│  │  ┌───────┴───────┐                                      │   │
│  │  │ Internet GW   │                                      │   │
│  │  └───────────────┘                                      │   │
│  │                                                         │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘

Key Components:
- VPC: Isolated virtual network
- Subnet: IP range within VPC
- Internet Gateway: Connects VPC to internet
- NAT Gateway: Allows private subnet outbound internet
- Route Table: Routing rules for subnets
- Security Group: Instance-level firewall (stateful)
- NACL: Subnet-level firewall (stateless)
```

### Security Groups vs NACLs

| Feature | Security Group | NACL |
|---------|---------------|------|
| Level | Instance | Subnet |
| State | Stateful | Stateless |
| Rules | Allow only | Allow & Deny |
| Evaluation | All rules | Order matters |
| Default | Deny all inbound | Allow all |

### Kubernetes Networking

```
┌─────────────────────────────────────────────────────────────────┐
│                    KUBERNETES NETWORKING                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  1. Pod-to-Pod Communication (Same Node)                        │
│     ┌────────────────────────────────────┐                      │
│     │  Node                              │                      │
│     │  ┌────────┐      ┌────────┐        │                      │
│     │  │ Pod A  │ ───► │ Pod B  │        │                      │
│     │  └────────┘      └────────┘        │                      │
│     │         │              │           │                      │
│     │         └──────┬───────┘           │                      │
│     │           veth pairs               │                      │
│     │         ┌──────┴───────┐           │                      │
│     │         │ Linux Bridge │           │                      │
│     │         └──────────────┘           │                      │
│     └────────────────────────────────────┘                      │
│                                                                 │
│  2. Service Types                                               │
│     - ClusterIP: Internal only (default)                        │
│     - NodePort: Exposes on node's IP:port                       │
│     - LoadBalancer: External load balancer                      │
│     - ExternalName: DNS alias                                   │
│                                                                 │
│  3. Ingress                                                     │
│     ┌──────────┐                                                │
│     │ Internet │                                                │
│     └────┬─────┘                                                │
│          ▼                                                      │
│     ┌──────────────┐                                            │
│     │   Ingress    │  (L7 routing)                              │
│     │  Controller  │  /api → service-a                          │
│     └──────────────┘  /web → service-b                          │
│          │                                                      │
│     ┌────┴────┐                                                 │
│     ▼         ▼                                                 │
│ ┌───────┐ ┌───────┐                                             │
│ │Svc A  │ │Svc B  │                                             │
│ └───────┘ └───────┘                                             │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Interview Questions & Answers

### Basic Questions

**Q1: What is the difference between TCP and UDP?**

**Answer:**
| TCP | UDP |
|-----|-----|
| Connection-oriented | Connectionless |
| Reliable delivery | Best-effort delivery |
| Ordered packets | No ordering |
| Error recovery | No recovery |
| Flow control | No flow control |
| Slower (overhead) | Faster |
| HTTP, FTP, SMTP | DNS, DHCP, VoIP |

---

**Q2: What happens when you type google.com in browser?**

**Answer:**
1. **DNS Resolution**: Browser checks cache → OS cache → Router → ISP DNS → Root → TLD → Authoritative DNS
2. **TCP Connection**: Three-way handshake with server
3. **TLS Handshake**: If HTTPS, establish secure connection
4. **HTTP Request**: Send GET request for /
5. **Server Processing**: Server processes request
6. **HTTP Response**: Server sends HTML response
7. **Browser Rendering**: Parse HTML, load CSS/JS, render page
8. **Connection Close**: TCP connection closed (or kept alive)

---

**Q3: Explain the OSI model layers with examples.**

**Answer:**
```
Layer 7 - Application:  HTTP, FTP (User interaction)
Layer 6 - Presentation: SSL, JPEG (Data format)
Layer 5 - Session:      NetBIOS (Session management)
Layer 4 - Transport:    TCP, UDP (End-to-end delivery)
Layer 3 - Network:      IP, ICMP (Routing)
Layer 2 - Data Link:    Ethernet (MAC addressing)
Layer 1 - Physical:     Cables (Bit transmission)
```

---

**Q4: What is the difference between Switch and Router?**

**Answer:**
| Switch (Layer 2) | Router (Layer 3) |
|------------------|------------------|
| Uses MAC addresses | Uses IP addresses |
| Connects devices in LAN | Connects different networks |
| Forwards frames | Routes packets |
| No IP configuration | Has IP address per interface |
| Creates single broadcast domain | Separates broadcast domains |

---

**Q5: What is ARP and how does it work?**

**Answer:**
ARP (Address Resolution Protocol) maps IP addresses to MAC addresses.

Process:
1. Host A knows IP of Host B, needs MAC
2. Host A broadcasts ARP Request: "Who has 192.168.1.2?"
3. Host B responds with ARP Reply: "192.168.1.2 is at AA:BB:CC:DD:EE:FF"
4. Host A caches this in ARP table
5. Communication proceeds using MAC address

---

### Intermediate Questions

**Q6: Explain subnetting with an example.**

**Answer:**
Given: 192.168.10.0/24, need 4 subnets

1. Bits needed: 2^2 = 4, need 2 bits
2. New mask: /24 + 2 = /26 (255.255.255.192)
3. Block size: 256 - 192 = 64

Subnets:
- 192.168.10.0/26 (hosts: 1-62)
- 192.168.10.64/26 (hosts: 65-126)
- 192.168.10.128/26 (hosts: 129-190)
- 192.168.10.192/26 (hosts: 193-254)

---

**Q7: What is the difference between Forward Proxy and Reverse Proxy?**

**Answer:**
**Forward Proxy:**
- Sits in front of clients
- Client knows about proxy
- Hides client identity from server
- Use: Corporate firewall, content filtering

**Reverse Proxy:**
- Sits in front of servers
- Client doesn't know backend servers
- Hides server identity from client
- Use: Load balancing, SSL termination, caching

---

**Q8: Explain DHCP DORA process.**

**Answer:**
1. **Discover**: Client broadcasts "I need an IP"
2. **Offer**: Server offers an IP address
3. **Request**: Client requests the offered IP
4. **Acknowledge**: Server confirms the lease

Additional info provided: IP, subnet mask, gateway, DNS, lease time

---

**Q9: What is NAT and what are its types?**

**Answer:**
NAT (Network Address Translation) translates private IPs to public IPs.

Types:
1. **Static NAT**: 1:1 mapping (one private to one public)
2. **Dynamic NAT**: Pool of public IPs, assigned as needed
3. **PAT/NAPT**: Many-to-one using different ports (most common)

---

**Q10: How does DNS work?**

**Answer:**
1. User types www.example.com
2. Browser checks local cache
3. OS checks hosts file and DNS cache
4. Query sent to recursive resolver (ISP)
5. Resolver queries root server → gets .com TLD
6. Resolver queries .com TLD → gets example.com NS
7. Resolver queries authoritative NS → gets IP
8. IP returned to client, cached at each level

---

### Advanced Questions

**Q11: Explain TCP connection termination (4-way handshake).**

**Answer:**
```
Client                  Server
  |                       |
  |-------- FIN --------->|  (Client wants to close)
  |                       |
  |<------- ACK ----------|  (Server acknowledges)
  |                       |
  |<------- FIN ----------|  (Server ready to close)
  |                       |
  |-------- ACK --------->|  (Client acknowledges)
  |                       |

TIME_WAIT: Client waits 2*MSL before closing
           to handle delayed packets
```

---

**Q12: What is the difference between Layer 4 and Layer 7 load balancing?**

**Answer:**
**Layer 4:**
- Works at transport layer (TCP/UDP)
- Routes based on IP and port
- Cannot inspect HTTP content
- Very fast, low latency
- Example: AWS NLB

**Layer 7:**
- Works at application layer (HTTP)
- Routes based on URL, headers, cookies
- Can do SSL termination
- Content-based routing
- Example: AWS ALB, Nginx

---

**Q13: What is BGP and why is it important?**

**Answer:**
BGP (Border Gateway Protocol) is the routing protocol of the internet.

- Path vector protocol
- Routes between Autonomous Systems (AS)
- Makes routing decisions based on policies
- Uses TCP port 179
- eBGP: Between different AS
- iBGP: Within same AS

Important because: It's how ISPs and large networks exchange routing information. A BGP misconfiguration can cause widespread internet outages.

---

**Q14: Explain the TLS handshake process.**

**Answer:**
1. **ClientHello**: Client sends supported ciphers, TLS version
2. **ServerHello**: Server selects cipher, sends certificate
3. **Certificate Verification**: Client verifies server certificate
4. **Key Exchange**: Client generates pre-master secret, encrypts with server's public key
5. **Session Keys**: Both derive symmetric session keys
6. **Finished**: Both send encrypted "Finished" message
7. **Secure Communication**: Data encrypted with session keys

---

**Q15: What are the differences between HTTP/1.1, HTTP/2, and HTTP/3?**

**Answer:**
**HTTP/1.1:**
- Text-based protocol
- One request per connection
- Head-of-line blocking
- Multiple connections needed

**HTTP/2:**
- Binary protocol
- Multiplexed streams over single connection
- Server push capability
- Header compression (HPACK)
- Still TCP-based, HOL blocking at TCP level

**HTTP/3:**
- Built on QUIC (UDP-based)
- No TCP head-of-line blocking
- 0-RTT connection resume
- Built-in encryption
- Connection migration (change IP/port)

---

**Q16: How would you troubleshoot a "website not loading" issue?**

**Answer:**
1. **Check basic connectivity**: `ping localhost` → verify TCP/IP stack
2. **Check gateway**: `ping <gateway>` → verify local network
3. **Check internet**: `ping 8.8.8.8` → verify internet connectivity
4. **Check DNS**: `nslookup domain` → verify DNS resolution
5. **Check specific port**: `telnet domain 80/443` → verify service accessible
6. **Check route**: `traceroute domain` → identify where packets drop
7. **Check firewall**: Verify no rules blocking traffic
8. **Check application**: Browser dev tools, curl -v

---

**Q17: Explain the concept of VLAN and its benefits.**

**Answer:**
VLAN (Virtual LAN) segments a physical network into logical networks.

**Benefits:**
- **Security**: Isolate sensitive traffic
- **Performance**: Reduce broadcast domain size
- **Flexibility**: Group by function, not location
- **Cost**: Use single switch for multiple networks

**How it works:**
- Frames tagged with VLAN ID (802.1Q)
- Switch ports assigned to VLANs
- Inter-VLAN routing requires Layer 3 device

---

**Q18: What is the difference between Symmetric and Asymmetric encryption?**

**Answer:**
**Symmetric (AES, DES):**
- Same key for encrypt/decrypt
- Fast, efficient
- Key distribution problem
- Used for bulk data encryption

**Asymmetric (RSA, ECC):**
- Public key encrypts, private key decrypts
- Slower, computationally intensive
- Solves key distribution
- Used for key exchange, digital signatures

**In TLS**: Asymmetric for key exchange, symmetric for data transfer

---

**Q19: Explain Zero Trust Network Architecture.**

**Answer:**
Zero Trust assumes no implicit trust based on network location.

**Principles:**
1. Never trust, always verify
2. Least privilege access
3. Micro-segmentation
4. Continuous verification
5. Assume breach

**Implementation:**
- Strong identity verification (MFA)
- Device health checks
- Encrypted communications
- Granular access controls
- Continuous monitoring

---

**Q20: What is a CDN and how does it improve performance?**

**Answer:**
CDN (Content Delivery Network) distributes content across geographic edge servers.

**How it works:**
1. User requests content
2. DNS routes to nearest edge server
3. Edge serves cached content
4. Cache miss: edge fetches from origin

**Benefits:**
- Reduced latency (geographic proximity)
- Reduced origin server load
- DDoS protection
- High availability
- SSL offloading

**Cached content:** Static files (images, CSS, JS), sometimes dynamic content

---

### Scenario-Based Questions

**Q21: Design a highly available web application architecture.**

**Answer:**
```
                    ┌─────────────┐
                    │    CDN      │
                    └──────┬──────┘
                           │
                    ┌──────┴──────┐
                    │   WAF/DDoS  │
                    └──────┬──────┘
                           │
                    ┌──────┴──────┐
                    │     ALB     │  (Load Balancer)
                    └──────┬──────┘
               ┌───────────┼───────────┐
               │           │           │
         ┌─────┴─────┐┌────┴────┐┌─────┴─────┐
         │  Web 1   ││  Web 2  ││  Web 3    │  (Auto-scaling)
         └─────┬─────┘└────┬────┘└─────┬─────┘
               │           │           │
         ┌─────┴───────────┴───────────┴─────┐
         │          Internal LB              │
         └─────────────────┬─────────────────┘
               ┌───────────┴───────────┐
         ┌─────┴─────┐           ┌─────┴─────┐
         │   App 1   │           │   App 2   │
         └─────┬─────┘           └─────┬─────┘
               │                       │
         ┌─────┴───────────────────────┴─────┐
         │        Database (Primary)         │
         │         + Read Replicas           │
         │         + Automatic failover      │
         └───────────────────────────────────┘

Key Components:
- Multi-AZ deployment
- Auto-scaling groups
- Database replication
- Health checks
- Monitoring/Alerting
```

---

**Q22: A user reports intermittent connectivity. How do you diagnose?**

**Answer:**
1. **Gather Information:**
   - When does it happen?
   - Specific sites or all?
   - Wired or wireless?
   - Other users affected?

2. **Check Physical:**
   - Cable connections
   - NIC status
   - Switch port lights

3. **Check Network:**
   - `ipconfig /all` - verify IP settings
   - `ping gateway` - test local connectivity
   - `ping 8.8.8.8` - test internet
   - `traceroute` - identify problem hop

4. **Check for Patterns:**
   - Packet loss: `ping -t` (continuous)
   - High latency spikes
   - DNS issues: try IP directly

5. **Advanced:**
   - Wireshark capture
   - Check for duplex mismatch
   - Look for spanning tree issues
   - Check switch/router logs

---

**Q23: How would you secure a web application at the network level?**

**Answer:**
1. **Edge Security:**
   - CDN with DDoS protection
   - Web Application Firewall (WAF)
   - Rate limiting

2. **Network Segmentation:**
   - VPC with public/private subnets
   - Web tier in public subnet
   - App/DB in private subnet
   - Bastion host for admin access

3. **Access Control:**
   - Security groups (whitelist approach)
   - Network ACLs
   - VPN for admin access

4. **Encryption:**
   - TLS everywhere
   - Certificate management
   - Encrypt data at rest

5. **Monitoring:**
   - Network flow logs
   - IDS/IPS
   - Security information and event management (SIEM)

---

## Quick Reference Card

### Common Ports
```
20/21 - FTP          22 - SSH           23 - Telnet
25 - SMTP            53 - DNS           67/68 - DHCP
80 - HTTP            110 - POP3         143 - IMAP
443 - HTTPS          3306 - MySQL       3389 - RDP
5432 - PostgreSQL    6379 - Redis       27017 - MongoDB
```

### Subnet Cheat Sheet
```
/30 = 4 hosts   (255.255.255.252)
/29 = 8 hosts   (255.255.255.248)
/28 = 16 hosts  (255.255.255.240)
/27 = 32 hosts  (255.255.255.224)
/26 = 64 hosts  (255.255.255.192)
/25 = 128 hosts (255.255.255.128)
/24 = 256 hosts (255.255.255.0)
/16 = 65536     (255.255.0.0)
/8 = 16M        (255.0.0.0)
```

### OSI Quick Reference
```
7 Application  - HTTP, FTP, DNS
6 Presentation - SSL, JPEG
5 Session      - NetBIOS
4 Transport    - TCP, UDP
3 Network      - IP, ICMP, ARP
2 Data Link    - Ethernet, MAC
1 Physical     - Cables, Hubs
```

### TCP Flags
```
SYN - Synchronize (connection start)
ACK - Acknowledge
FIN - Finish (connection end)
RST - Reset (abort connection)
PSH - Push (send immediately)
URG - Urgent data
```

---

## Conclusion

This guide covers networking concepts from basic to advanced levels. Key areas to focus on for interviews:

1. **Fundamentals**: OSI model, TCP vs UDP, IP addressing
2. **Protocols**: HTTP/HTTPS, DNS, DHCP, ARP
3. **Security**: Firewalls, VPN, TLS, common attacks
4. **Cloud**: AWS VPC, Kubernetes networking
5. **Troubleshooting**: Diagnostic commands, systematic approach

Remember to practice hands-on with tools like Wireshark, and understand not just what things are, but why they exist and when to use them.
