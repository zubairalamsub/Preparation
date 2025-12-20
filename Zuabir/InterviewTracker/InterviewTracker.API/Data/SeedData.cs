using InterviewTracker.API.Models;

namespace InterviewTracker.API.Data;

public static class SeedData
{
    public static List<DSAProblem> GetDSAProblems()
    {
        return new List<DSAProblem>
        {
            // Arrays
            new() { Title = "Two Sum", Category = "Array", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 1, ProblemUrl = "https://leetcode.com/problems/two-sum/", Tags = new() { "Hash Table", "Array" } },
            new() { Title = "Best Time to Buy and Sell Stock", Category = "Array", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 121, ProblemUrl = "https://leetcode.com/problems/best-time-to-buy-and-sell-stock/", Tags = new() { "Array", "DP" } },
            new() { Title = "Contains Duplicate", Category = "Array", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 217, ProblemUrl = "https://leetcode.com/problems/contains-duplicate/", Tags = new() { "Array", "Hash Table" } },
            new() { Title = "Product of Array Except Self", Category = "Array", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 238, ProblemUrl = "https://leetcode.com/problems/product-of-array-except-self/", Tags = new() { "Array", "Prefix Sum" } },
            new() { Title = "Maximum Subarray", Category = "Array", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 53, ProblemUrl = "https://leetcode.com/problems/maximum-subarray/", Tags = new() { "Array", "DP", "Kadane" } },
            new() { Title = "Maximum Product Subarray", Category = "Array", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 152, ProblemUrl = "https://leetcode.com/problems/maximum-product-subarray/", Tags = new() { "Array", "DP" } },
            new() { Title = "Find Minimum in Rotated Sorted Array", Category = "Array", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 153, ProblemUrl = "https://leetcode.com/problems/find-minimum-in-rotated-sorted-array/", Tags = new() { "Array", "Binary Search" } },
            new() { Title = "Search in Rotated Sorted Array", Category = "Array", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 33, ProblemUrl = "https://leetcode.com/problems/search-in-rotated-sorted-array/", Tags = new() { "Array", "Binary Search" } },
            new() { Title = "3Sum", Category = "Array", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 15, ProblemUrl = "https://leetcode.com/problems/3sum/", Tags = new() { "Array", "Two Pointers" } },
            new() { Title = "Container With Most Water", Category = "Array", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 11, ProblemUrl = "https://leetcode.com/problems/container-with-most-water/", Tags = new() { "Array", "Two Pointers" } },

            // Strings
            new() { Title = "Valid Anagram", Category = "String", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 242, ProblemUrl = "https://leetcode.com/problems/valid-anagram/", Tags = new() { "String", "Hash Table" } },
            new() { Title = "Valid Palindrome", Category = "String", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 125, ProblemUrl = "https://leetcode.com/problems/valid-palindrome/", Tags = new() { "String", "Two Pointers" } },
            new() { Title = "Longest Substring Without Repeating Characters", Category = "String", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 3, ProblemUrl = "https://leetcode.com/problems/longest-substring-without-repeating-characters/", Tags = new() { "String", "Sliding Window" } },
            new() { Title = "Longest Palindromic Substring", Category = "String", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 5, ProblemUrl = "https://leetcode.com/problems/longest-palindromic-substring/", Tags = new() { "String", "DP" } },
            new() { Title = "Group Anagrams", Category = "String", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 49, ProblemUrl = "https://leetcode.com/problems/group-anagrams/", Tags = new() { "String", "Hash Table" } },
            new() { Title = "Longest Repeating Character Replacement", Category = "String", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 424, ProblemUrl = "https://leetcode.com/problems/longest-repeating-character-replacement/", Tags = new() { "String", "Sliding Window" } },
            new() { Title = "Minimum Window Substring", Category = "String", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 76, ProblemUrl = "https://leetcode.com/problems/minimum-window-substring/", Tags = new() { "String", "Sliding Window" } },

            // Linked List
            new() { Title = "Reverse Linked List", Category = "Linked List", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 206, ProblemUrl = "https://leetcode.com/problems/reverse-linked-list/", Tags = new() { "Linked List" } },
            new() { Title = "Merge Two Sorted Lists", Category = "Linked List", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 21, ProblemUrl = "https://leetcode.com/problems/merge-two-sorted-lists/", Tags = new() { "Linked List" } },
            new() { Title = "Linked List Cycle", Category = "Linked List", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 141, ProblemUrl = "https://leetcode.com/problems/linked-list-cycle/", Tags = new() { "Linked List", "Two Pointers" } },
            new() { Title = "Reorder List", Category = "Linked List", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 143, ProblemUrl = "https://leetcode.com/problems/reorder-list/", Tags = new() { "Linked List" } },
            new() { Title = "Remove Nth Node From End of List", Category = "Linked List", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 19, ProblemUrl = "https://leetcode.com/problems/remove-nth-node-from-end-of-list/", Tags = new() { "Linked List", "Two Pointers" } },
            new() { Title = "Merge K Sorted Lists", Category = "Linked List", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 23, ProblemUrl = "https://leetcode.com/problems/merge-k-sorted-lists/", Tags = new() { "Linked List", "Heap" } },

            // Trees
            new() { Title = "Invert Binary Tree", Category = "Tree", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 226, ProblemUrl = "https://leetcode.com/problems/invert-binary-tree/", Tags = new() { "Tree", "BFS", "DFS" } },
            new() { Title = "Maximum Depth of Binary Tree", Category = "Tree", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 104, ProblemUrl = "https://leetcode.com/problems/maximum-depth-of-binary-tree/", Tags = new() { "Tree", "DFS" } },
            new() { Title = "Same Tree", Category = "Tree", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 100, ProblemUrl = "https://leetcode.com/problems/same-tree/", Tags = new() { "Tree", "DFS" } },
            new() { Title = "Subtree of Another Tree", Category = "Tree", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 572, ProblemUrl = "https://leetcode.com/problems/subtree-of-another-tree/", Tags = new() { "Tree", "DFS" } },
            new() { Title = "Lowest Common Ancestor of a BST", Category = "Tree", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 235, ProblemUrl = "https://leetcode.com/problems/lowest-common-ancestor-of-a-binary-search-tree/", Tags = new() { "Tree", "BST" } },
            new() { Title = "Binary Tree Level Order Traversal", Category = "Tree", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 102, ProblemUrl = "https://leetcode.com/problems/binary-tree-level-order-traversal/", Tags = new() { "Tree", "BFS" } },
            new() { Title = "Validate Binary Search Tree", Category = "Tree", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 98, ProblemUrl = "https://leetcode.com/problems/validate-binary-search-tree/", Tags = new() { "Tree", "BST" } },
            new() { Title = "Kth Smallest Element in a BST", Category = "Tree", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 230, ProblemUrl = "https://leetcode.com/problems/kth-smallest-element-in-a-bst/", Tags = new() { "Tree", "BST" } },
            new() { Title = "Construct Binary Tree from Preorder and Inorder", Category = "Tree", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 105, ProblemUrl = "https://leetcode.com/problems/construct-binary-tree-from-preorder-and-inorder-traversal/", Tags = new() { "Tree", "DFS" } },
            new() { Title = "Binary Tree Maximum Path Sum", Category = "Tree", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 124, ProblemUrl = "https://leetcode.com/problems/binary-tree-maximum-path-sum/", Tags = new() { "Tree", "DFS" } },
            new() { Title = "Serialize and Deserialize Binary Tree", Category = "Tree", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 297, ProblemUrl = "https://leetcode.com/problems/serialize-and-deserialize-binary-tree/", Tags = new() { "Tree", "BFS", "DFS" } },

            // Dynamic Programming
            new() { Title = "Climbing Stairs", Category = "Dynamic Programming", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 70, ProblemUrl = "https://leetcode.com/problems/climbing-stairs/", Tags = new() { "DP" } },
            new() { Title = "House Robber", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 198, ProblemUrl = "https://leetcode.com/problems/house-robber/", Tags = new() { "DP" } },
            new() { Title = "House Robber II", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 213, ProblemUrl = "https://leetcode.com/problems/house-robber-ii/", Tags = new() { "DP" } },
            new() { Title = "Longest Increasing Subsequence", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 300, ProblemUrl = "https://leetcode.com/problems/longest-increasing-subsequence/", Tags = new() { "DP", "Binary Search" } },
            new() { Title = "Coin Change", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 322, ProblemUrl = "https://leetcode.com/problems/coin-change/", Tags = new() { "DP" } },
            new() { Title = "Longest Common Subsequence", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 1143, ProblemUrl = "https://leetcode.com/problems/longest-common-subsequence/", Tags = new() { "DP" } },
            new() { Title = "Word Break", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 139, ProblemUrl = "https://leetcode.com/problems/word-break/", Tags = new() { "DP", "Hash Table" } },
            new() { Title = "Combination Sum IV", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 377, ProblemUrl = "https://leetcode.com/problems/combination-sum-iv/", Tags = new() { "DP" } },
            new() { Title = "Decode Ways", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 91, ProblemUrl = "https://leetcode.com/problems/decode-ways/", Tags = new() { "DP", "String" } },
            new() { Title = "Unique Paths", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 62, ProblemUrl = "https://leetcode.com/problems/unique-paths/", Tags = new() { "DP" } },
            new() { Title = "Jump Game", Category = "Dynamic Programming", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 55, ProblemUrl = "https://leetcode.com/problems/jump-game/", Tags = new() { "DP", "Greedy" } },

            // Graph
            new() { Title = "Number of Islands", Category = "Graph", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 200, ProblemUrl = "https://leetcode.com/problems/number-of-islands/", Tags = new() { "Graph", "DFS", "BFS" } },
            new() { Title = "Clone Graph", Category = "Graph", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 133, ProblemUrl = "https://leetcode.com/problems/clone-graph/", Tags = new() { "Graph", "DFS", "BFS" } },
            new() { Title = "Pacific Atlantic Water Flow", Category = "Graph", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 417, ProblemUrl = "https://leetcode.com/problems/pacific-atlantic-water-flow/", Tags = new() { "Graph", "DFS" } },
            new() { Title = "Course Schedule", Category = "Graph", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 207, ProblemUrl = "https://leetcode.com/problems/course-schedule/", Tags = new() { "Graph", "Topological Sort" } },
            new() { Title = "Course Schedule II", Category = "Graph", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 210, ProblemUrl = "https://leetcode.com/problems/course-schedule-ii/", Tags = new() { "Graph", "Topological Sort" } },
            new() { Title = "Graph Valid Tree", Category = "Graph", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 261, ProblemUrl = "https://leetcode.com/problems/graph-valid-tree/", Tags = new() { "Graph", "Union Find" } },
            new() { Title = "Number of Connected Components", Category = "Graph", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 323, ProblemUrl = "https://leetcode.com/problems/number-of-connected-components-in-an-undirected-graph/", Tags = new() { "Graph", "Union Find" } },
            new() { Title = "Alien Dictionary", Category = "Graph", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 269, ProblemUrl = "https://leetcode.com/problems/alien-dictionary/", Tags = new() { "Graph", "Topological Sort" } },

            // Backtracking
            new() { Title = "Subsets", Category = "Backtracking", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 78, ProblemUrl = "https://leetcode.com/problems/subsets/", Tags = new() { "Backtracking", "Bit Manipulation" } },
            new() { Title = "Combination Sum", Category = "Backtracking", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 39, ProblemUrl = "https://leetcode.com/problems/combination-sum/", Tags = new() { "Backtracking" } },
            new() { Title = "Permutations", Category = "Backtracking", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 46, ProblemUrl = "https://leetcode.com/problems/permutations/", Tags = new() { "Backtracking" } },
            new() { Title = "Word Search", Category = "Backtracking", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 79, ProblemUrl = "https://leetcode.com/problems/word-search/", Tags = new() { "Backtracking", "Matrix" } },
            new() { Title = "Palindrome Partitioning", Category = "Backtracking", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 131, ProblemUrl = "https://leetcode.com/problems/palindrome-partitioning/", Tags = new() { "Backtracking", "DP" } },
            new() { Title = "Letter Combinations of a Phone Number", Category = "Backtracking", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 17, ProblemUrl = "https://leetcode.com/problems/letter-combinations-of-a-phone-number/", Tags = new() { "Backtracking" } },
            new() { Title = "N-Queens", Category = "Backtracking", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 51, ProblemUrl = "https://leetcode.com/problems/n-queens/", Tags = new() { "Backtracking" } },

            // Heap / Priority Queue
            new() { Title = "Top K Frequent Elements", Category = "Heap", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 347, ProblemUrl = "https://leetcode.com/problems/top-k-frequent-elements/", Tags = new() { "Heap", "Hash Table" } },
            new() { Title = "Find Median from Data Stream", Category = "Heap", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 295, ProblemUrl = "https://leetcode.com/problems/find-median-from-data-stream/", Tags = new() { "Heap", "Design" } },

            // Binary Search
            new() { Title = "Binary Search", Category = "Binary Search", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 704, ProblemUrl = "https://leetcode.com/problems/binary-search/", Tags = new() { "Binary Search" } },
            new() { Title = "Search a 2D Matrix", Category = "Binary Search", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 74, ProblemUrl = "https://leetcode.com/problems/search-a-2d-matrix/", Tags = new() { "Binary Search", "Matrix" } },
            new() { Title = "Koko Eating Bananas", Category = "Binary Search", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 875, ProblemUrl = "https://leetcode.com/problems/koko-eating-bananas/", Tags = new() { "Binary Search" } },
            new() { Title = "Median of Two Sorted Arrays", Category = "Binary Search", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 4, ProblemUrl = "https://leetcode.com/problems/median-of-two-sorted-arrays/", Tags = new() { "Binary Search" } },

            // Stack
            new() { Title = "Valid Parentheses", Category = "Stack", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 20, ProblemUrl = "https://leetcode.com/problems/valid-parentheses/", Tags = new() { "Stack" } },
            new() { Title = "Min Stack", Category = "Stack", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 155, ProblemUrl = "https://leetcode.com/problems/min-stack/", Tags = new() { "Stack", "Design" } },
            new() { Title = "Evaluate Reverse Polish Notation", Category = "Stack", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 150, ProblemUrl = "https://leetcode.com/problems/evaluate-reverse-polish-notation/", Tags = new() { "Stack" } },
            new() { Title = "Daily Temperatures", Category = "Stack", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 739, ProblemUrl = "https://leetcode.com/problems/daily-temperatures/", Tags = new() { "Stack", "Monotonic Stack" } },
            new() { Title = "Largest Rectangle in Histogram", Category = "Stack", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 84, ProblemUrl = "https://leetcode.com/problems/largest-rectangle-in-histogram/", Tags = new() { "Stack", "Monotonic Stack" } },

            // Intervals
            new() { Title = "Meeting Rooms", Category = "Intervals", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 252, ProblemUrl = "https://leetcode.com/problems/meeting-rooms/", Tags = new() { "Intervals", "Sorting" } },
            new() { Title = "Merge Intervals", Category = "Intervals", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 56, ProblemUrl = "https://leetcode.com/problems/merge-intervals/", Tags = new() { "Intervals", "Sorting" } },
            new() { Title = "Insert Interval", Category = "Intervals", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 57, ProblemUrl = "https://leetcode.com/problems/insert-interval/", Tags = new() { "Intervals" } },
            new() { Title = "Non-overlapping Intervals", Category = "Intervals", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 435, ProblemUrl = "https://leetcode.com/problems/non-overlapping-intervals/", Tags = new() { "Intervals", "Greedy" } },
            new() { Title = "Meeting Rooms II", Category = "Intervals", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 253, ProblemUrl = "https://leetcode.com/problems/meeting-rooms-ii/", Tags = new() { "Intervals", "Heap" } },

            // Bit Manipulation
            new() { Title = "Number of 1 Bits", Category = "Bit Manipulation", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 191, ProblemUrl = "https://leetcode.com/problems/number-of-1-bits/", Tags = new() { "Bit Manipulation" } },
            new() { Title = "Counting Bits", Category = "Bit Manipulation", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 338, ProblemUrl = "https://leetcode.com/problems/counting-bits/", Tags = new() { "Bit Manipulation", "DP" } },
            new() { Title = "Missing Number", Category = "Bit Manipulation", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 268, ProblemUrl = "https://leetcode.com/problems/missing-number/", Tags = new() { "Bit Manipulation" } },
            new() { Title = "Reverse Bits", Category = "Bit Manipulation", Difficulty = "Easy", Platform = "LeetCode", LeetCodeNumber = 190, ProblemUrl = "https://leetcode.com/problems/reverse-bits/", Tags = new() { "Bit Manipulation" } },
            new() { Title = "Sum of Two Integers", Category = "Bit Manipulation", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 371, ProblemUrl = "https://leetcode.com/problems/sum-of-two-integers/", Tags = new() { "Bit Manipulation" } },

            // Tries
            new() { Title = "Implement Trie (Prefix Tree)", Category = "Trie", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 208, ProblemUrl = "https://leetcode.com/problems/implement-trie-prefix-tree/", Tags = new() { "Trie", "Design" } },
            new() { Title = "Design Add and Search Words Data Structure", Category = "Trie", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 211, ProblemUrl = "https://leetcode.com/problems/design-add-and-search-words-data-structure/", Tags = new() { "Trie", "Design" } },
            new() { Title = "Word Search II", Category = "Trie", Difficulty = "Hard", Platform = "LeetCode", LeetCodeNumber = 212, ProblemUrl = "https://leetcode.com/problems/word-search-ii/", Tags = new() { "Trie", "Backtracking" } },

            // Math & Geometry
            new() { Title = "Rotate Image", Category = "Math", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 48, ProblemUrl = "https://leetcode.com/problems/rotate-image/", Tags = new() { "Matrix", "Math" } },
            new() { Title = "Spiral Matrix", Category = "Math", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 54, ProblemUrl = "https://leetcode.com/problems/spiral-matrix/", Tags = new() { "Matrix" } },
            new() { Title = "Set Matrix Zeroes", Category = "Math", Difficulty = "Medium", Platform = "LeetCode", LeetCodeNumber = 73, ProblemUrl = "https://leetcode.com/problems/set-matrix-zeroes/", Tags = new() { "Matrix" } },
        };
    }

    public static List<SystemDesignTopic> GetSystemDesignTopics()
    {
        return new List<SystemDesignTopic>
        {
            // Fundamentals
            new() { Title = "CAP Theorem", Category = "Fundamentals", Difficulty = "Medium", KeyConcepts = "Consistency, Availability, Partition Tolerance - You can only have 2 out of 3", Resources = "https://www.youtube.com/watch?v=k-Yaq8AHlFA" },
            new() { Title = "ACID Properties", Category = "Fundamentals", Difficulty = "Easy", KeyConcepts = "Atomicity, Consistency, Isolation, Durability - Database transaction properties" },
            new() { Title = "BASE Properties", Category = "Fundamentals", Difficulty = "Medium", KeyConcepts = "Basically Available, Soft state, Eventually consistent - NoSQL alternative to ACID" },
            new() { Title = "Vertical vs Horizontal Scaling", Category = "Fundamentals", Difficulty = "Easy", KeyConcepts = "Scale up (bigger machine) vs Scale out (more machines)" },
            new() { Title = "Consistent Hashing", Category = "Fundamentals", Difficulty = "Medium", KeyConcepts = "Distributed hash table technique for minimal reorganization when nodes change" },

            // Load Balancing
            new() { Title = "Load Balancer Basics", Category = "Load Balancing", Difficulty = "Easy", KeyConcepts = "Round Robin, Least Connections, IP Hash, Weighted algorithms" },
            new() { Title = "L4 vs L7 Load Balancing", Category = "Load Balancing", Difficulty = "Medium", KeyConcepts = "Transport layer vs Application layer load balancing" },
            new() { Title = "Global Server Load Balancing (GSLB)", Category = "Load Balancing", Difficulty = "Medium", KeyConcepts = "GeoDNS, Anycast, latency-based routing" },

            // Caching
            new() { Title = "Cache Strategies", Category = "Caching", Difficulty = "Medium", KeyConcepts = "Cache-aside, Read-through, Write-through, Write-behind, Write-around" },
            new() { Title = "Cache Eviction Policies", Category = "Caching", Difficulty = "Easy", KeyConcepts = "LRU, LFU, FIFO, TTL-based eviction" },
            new() { Title = "Redis", Category = "Caching", Difficulty = "Medium", KeyConcepts = "In-memory data store, pub/sub, data structures, persistence options" },
            new() { Title = "Memcached", Category = "Caching", Difficulty = "Easy", KeyConcepts = "Simple key-value store, multi-threaded, no persistence" },
            new() { Title = "CDN (Content Delivery Network)", Category = "Caching", Difficulty = "Medium", KeyConcepts = "Edge caching, PoPs, origin servers, cache invalidation" },

            // Databases
            new() { Title = "SQL vs NoSQL", Category = "Database", Difficulty = "Easy", KeyConcepts = "Relational vs Non-relational, ACID vs BASE, Schema vs Schemaless" },
            new() { Title = "Database Sharding", Category = "Database", Difficulty = "Hard", KeyConcepts = "Horizontal partitioning, shard key selection, cross-shard queries" },
            new() { Title = "Database Replication", Category = "Database", Difficulty = "Medium", KeyConcepts = "Master-slave, Master-master, synchronous vs asynchronous replication" },
            new() { Title = "Database Indexing", Category = "Database", Difficulty = "Medium", KeyConcepts = "B-tree, Hash indexes, composite indexes, covering indexes" },
            new() { Title = "MongoDB", Category = "Database", Difficulty = "Medium", KeyConcepts = "Document store, flexible schema, replica sets, sharding" },
            new() { Title = "Cassandra", Category = "Database", Difficulty = "Hard", KeyConcepts = "Wide-column store, partition keys, clustering keys, gossip protocol" },
            new() { Title = "PostgreSQL", Category = "Database", Difficulty = "Medium", KeyConcepts = "ACID compliant, MVCC, JSON support, extensions" },

            // Message Queues
            new() { Title = "Message Queue Basics", Category = "Message Queues", Difficulty = "Medium", KeyConcepts = "Producer, Consumer, Queue, Topics, Pub/Sub vs Point-to-Point" },
            new() { Title = "Apache Kafka", Category = "Message Queues", Difficulty = "Hard", KeyConcepts = "Distributed log, partitions, consumer groups, exactly-once semantics" },
            new() { Title = "RabbitMQ", Category = "Message Queues", Difficulty = "Medium", KeyConcepts = "AMQP, exchanges, routing keys, acknowledgments" },
            new() { Title = "Amazon SQS", Category = "Message Queues", Difficulty = "Easy", KeyConcepts = "Fully managed, standard vs FIFO queues, visibility timeout" },

            // Microservices
            new() { Title = "Microservices Architecture", Category = "Microservices", Difficulty = "Medium", KeyConcepts = "Service decomposition, bounded contexts, independent deployment" },
            new() { Title = "API Gateway", Category = "Microservices", Difficulty = "Medium", KeyConcepts = "Request routing, authentication, rate limiting, aggregation" },
            new() { Title = "Service Discovery", Category = "Microservices", Difficulty = "Medium", KeyConcepts = "Client-side vs Server-side discovery, Consul, etcd, Eureka" },
            new() { Title = "Circuit Breaker Pattern", Category = "Microservices", Difficulty = "Medium", KeyConcepts = "Fault tolerance, fallback mechanisms, health checks" },
            new() { Title = "Saga Pattern", Category = "Microservices", Difficulty = "Hard", KeyConcepts = "Distributed transactions, choreography vs orchestration, compensating transactions" },
            new() { Title = "Event-Driven Architecture", Category = "Microservices", Difficulty = "Medium", KeyConcepts = "Event sourcing, CQRS, eventual consistency" },

            // API Design
            new() { Title = "REST API Design", Category = "API Design", Difficulty = "Easy", KeyConcepts = "Resources, HTTP methods, status codes, HATEOAS" },
            new() { Title = "GraphQL", Category = "API Design", Difficulty = "Medium", KeyConcepts = "Schema, queries, mutations, subscriptions, N+1 problem" },
            new() { Title = "gRPC", Category = "API Design", Difficulty = "Medium", KeyConcepts = "Protocol buffers, HTTP/2, bidirectional streaming" },
            new() { Title = "API Rate Limiting", Category = "API Design", Difficulty = "Medium", KeyConcepts = "Token bucket, leaky bucket, sliding window, fixed window" },
            new() { Title = "API Versioning", Category = "API Design", Difficulty = "Easy", KeyConcepts = "URL versioning, header versioning, query param versioning" },

            // Security
            new() { Title = "Authentication vs Authorization", Category = "Security", Difficulty = "Easy", KeyConcepts = "Who you are vs What you can do, OAuth, JWT, RBAC" },
            new() { Title = "OAuth 2.0", Category = "Security", Difficulty = "Medium", KeyConcepts = "Authorization flows, access tokens, refresh tokens, scopes" },
            new() { Title = "JWT (JSON Web Tokens)", Category = "Security", Difficulty = "Easy", KeyConcepts = "Header, payload, signature, stateless authentication" },
            new() { Title = "HTTPS/TLS", Category = "Security", Difficulty = "Medium", KeyConcepts = "SSL certificates, handshake, symmetric vs asymmetric encryption" },
            new() { Title = "SQL Injection Prevention", Category = "Security", Difficulty = "Easy", KeyConcepts = "Parameterized queries, input validation, ORM usage" },

            // Monitoring & Observability
            new() { Title = "Logging Best Practices", Category = "Monitoring", Difficulty = "Easy", KeyConcepts = "Structured logging, log levels, centralized logging (ELK)" },
            new() { Title = "Metrics & Monitoring", Category = "Monitoring", Difficulty = "Medium", KeyConcepts = "RED metrics, USE method, Prometheus, Grafana" },
            new() { Title = "Distributed Tracing", Category = "Monitoring", Difficulty = "Medium", KeyConcepts = "Trace ID, spans, Jaeger, Zipkin, correlation IDs" },
            new() { Title = "Alerting Strategies", Category = "Monitoring", Difficulty = "Easy", KeyConcepts = "Alert fatigue, SLOs, error budgets, PagerDuty" },

            // System Design Problems
            new() { Title = "Design URL Shortener", Category = "System Design Problems", Difficulty = "Easy", KeyConcepts = "Base62 encoding, hash collision, analytics, expiration" },
            new() { Title = "Design Twitter", Category = "System Design Problems", Difficulty = "Hard", KeyConcepts = "Fan-out, timeline generation, celebrity problem, caching" },
            new() { Title = "Design YouTube", Category = "System Design Problems", Difficulty = "Hard", KeyConcepts = "Video encoding, CDN, recommendation system, thumbnail generation" },
            new() { Title = "Design WhatsApp", Category = "System Design Problems", Difficulty = "Hard", KeyConcepts = "WebSockets, message queue, read receipts, encryption" },
            new() { Title = "Design Instagram", Category = "System Design Problems", Difficulty = "Hard", KeyConcepts = "Photo storage, news feed, stories, CDN" },
            new() { Title = "Design Uber/Lyft", Category = "System Design Problems", Difficulty = "Hard", KeyConcepts = "Location tracking, matching algorithm, surge pricing, maps" },
            new() { Title = "Design Rate Limiter", Category = "System Design Problems", Difficulty = "Medium", KeyConcepts = "Token bucket, sliding window, distributed rate limiting" },
            new() { Title = "Design Notification System", Category = "System Design Problems", Difficulty = "Medium", KeyConcepts = "Push vs Pull, templates, priority queues, delivery tracking" },
            new() { Title = "Design Pastebin", Category = "System Design Problems", Difficulty = "Easy", KeyConcepts = "Object storage, TTL, access patterns, CDN" },
            new() { Title = "Design Dropbox/Google Drive", Category = "System Design Problems", Difficulty = "Hard", KeyConcepts = "File chunking, deduplication, sync, conflict resolution" },
            new() { Title = "Design Search Autocomplete", Category = "System Design Problems", Difficulty = "Medium", KeyConcepts = "Trie, prefix matching, ranking, caching" },
            new() { Title = "Design Web Crawler", Category = "System Design Problems", Difficulty = "Medium", KeyConcepts = "BFS, URL frontier, politeness, deduplication" },
            new() { Title = "Design Chat System", Category = "System Design Problems", Difficulty = "Medium", KeyConcepts = "WebSockets, message ordering, presence, group chats" },
            new() { Title = "Design E-commerce Platform", Category = "System Design Problems", Difficulty = "Hard", KeyConcepts = "Product catalog, cart, inventory, payment processing" },
            new() { Title = "Design Ticketmaster", Category = "System Design Problems", Difficulty = "Hard", KeyConcepts = "Seat reservation, concurrency, waiting queue, payment" },
            new() { Title = "Design Google Maps", Category = "System Design Problems", Difficulty = "Hard", KeyConcepts = "Graph algorithms, tile serving, ETA calculation, routing" },
            new() { Title = "Design Yelp/Nearby Places", Category = "System Design Problems", Difficulty = "Medium", KeyConcepts = "Geospatial indexing, QuadTree, reviews, search ranking" },

            // Cloud & Infrastructure
            new() { Title = "Kubernetes Basics", Category = "Infrastructure", Difficulty = "Medium", KeyConcepts = "Pods, Services, Deployments, ConfigMaps, Secrets" },
            new() { Title = "Docker & Containerization", Category = "Infrastructure", Difficulty = "Easy", KeyConcepts = "Images, containers, Dockerfile, volumes, networking" },
            new() { Title = "CI/CD Pipelines", Category = "Infrastructure", Difficulty = "Easy", KeyConcepts = "Build, test, deploy automation, Jenkins, GitHub Actions" },
            new() { Title = "Infrastructure as Code", Category = "Infrastructure", Difficulty = "Medium", KeyConcepts = "Terraform, CloudFormation, Pulumi, immutable infrastructure" },
            new() { Title = "Service Mesh", Category = "Infrastructure", Difficulty = "Hard", KeyConcepts = "Istio, sidecar proxy, mTLS, traffic management" },

            // Data Engineering
            new() { Title = "Data Lake vs Data Warehouse", Category = "Data Engineering", Difficulty = "Medium", KeyConcepts = "Schema-on-read vs Schema-on-write, structured vs unstructured" },
            new() { Title = "ETL Pipelines", Category = "Data Engineering", Difficulty = "Medium", KeyConcepts = "Extract, Transform, Load, batch vs streaming, Apache Airflow" },
            new() { Title = "Apache Spark", Category = "Data Engineering", Difficulty = "Hard", KeyConcepts = "RDDs, DataFrames, Spark SQL, distributed processing" },
            new() { Title = "Stream Processing", Category = "Data Engineering", Difficulty = "Hard", KeyConcepts = "Apache Flink, Kafka Streams, windowing, state management" },

            // Real-time Systems
            new() { Title = "WebSockets", Category = "Real-time", Difficulty = "Medium", KeyConcepts = "Full-duplex communication, connection management, scaling" },
            new() { Title = "Server-Sent Events (SSE)", Category = "Real-time", Difficulty = "Easy", KeyConcepts = "One-way server push, reconnection, event types" },
            new() { Title = "Long Polling", Category = "Real-time", Difficulty = "Easy", KeyConcepts = "HTTP-based, timeout handling, comparison with WebSockets" },
            new() { Title = "Pub/Sub Systems", Category = "Real-time", Difficulty = "Medium", KeyConcepts = "Publishers, subscribers, topics, message filtering" },
        };
    }

    public static List<AzureTopic> GetAzureTopics()
    {
        return new List<AzureTopic>
        {
            // Compute Services
            new() { Title = "Azure Virtual Machines", Category = "Compute", Difficulty = "Easy", AzureService = "VM", KeyConcepts = "IaaS, VM sizes, availability sets, scale sets, managed disks", Tags = new() { "IaaS", "VM" } },
            new() { Title = "Azure App Service", Category = "Compute", Difficulty = "Easy", AzureService = "App Service", KeyConcepts = "PaaS, deployment slots, scaling, WebJobs, custom domains, SSL", Tags = new() { "PaaS", "Web Apps" } },
            new() { Title = "Azure Functions", Category = "Compute", Difficulty = "Medium", AzureService = "Functions", KeyConcepts = "Serverless, triggers, bindings, Durable Functions, consumption plan vs premium", Tags = new() { "Serverless", "FaaS" } },
            new() { Title = "Azure Kubernetes Service (AKS)", Category = "Compute", Difficulty = "Hard", AzureService = "AKS", KeyConcepts = "Container orchestration, pods, services, ingress, Helm, node pools", Tags = new() { "Kubernetes", "Containers" } },
            new() { Title = "Azure Container Instances", Category = "Compute", Difficulty = "Easy", AzureService = "ACI", KeyConcepts = "Serverless containers, quick deployment, container groups", Tags = new() { "Containers", "Serverless" } },
            new() { Title = "Azure Container Apps", Category = "Compute", Difficulty = "Medium", AzureService = "Container Apps", KeyConcepts = "Serverless containers, Dapr integration, KEDA scaling, revisions", Tags = new() { "Containers", "Serverless" } },
            new() { Title = "Azure Batch", Category = "Compute", Difficulty = "Medium", AzureService = "Batch", KeyConcepts = "Large-scale parallel computing, job scheduling, pools, tasks", Tags = new() { "HPC", "Batch Processing" } },

            // Storage Services
            new() { Title = "Azure Blob Storage", Category = "Storage", Difficulty = "Easy", AzureService = "Blob Storage", KeyConcepts = "Block, append, page blobs, access tiers (hot, cool, archive), lifecycle management", Tags = new() { "Object Storage" } },
            new() { Title = "Azure Files", Category = "Storage", Difficulty = "Easy", AzureService = "Azure Files", KeyConcepts = "SMB/NFS file shares, Azure File Sync, mounting in VMs", Tags = new() { "File Storage", "SMB" } },
            new() { Title = "Azure Queue Storage", Category = "Storage", Difficulty = "Easy", AzureService = "Queue Storage", KeyConcepts = "Simple message queuing, at-least-once delivery, visibility timeout", Tags = new() { "Messaging", "Queues" } },
            new() { Title = "Azure Table Storage", Category = "Storage", Difficulty = "Easy", AzureService = "Table Storage", KeyConcepts = "NoSQL key-value store, partition key, row key, entity properties", Tags = new() { "NoSQL" } },
            new() { Title = "Azure Data Lake Storage Gen2", Category = "Storage", Difficulty = "Medium", AzureService = "ADLS Gen2", KeyConcepts = "Hierarchical namespace, big data analytics, HDFS compatible, ACLs", Tags = new() { "Big Data", "Data Lake" } },
            new() { Title = "Azure Managed Disks", Category = "Storage", Difficulty = "Easy", AzureService = "Managed Disks", KeyConcepts = "Standard HDD, Standard SSD, Premium SSD, Ultra Disk, disk encryption", Tags = new() { "Block Storage" } },

            // Database Services
            new() { Title = "Azure SQL Database", Category = "Database", Difficulty = "Medium", AzureService = "SQL Database", KeyConcepts = "PaaS SQL Server, DTU vs vCore, elastic pools, geo-replication, auto-tuning", Tags = new() { "SQL", "Relational" } },
            new() { Title = "Azure Cosmos DB", Category = "Database", Difficulty = "Hard", AzureService = "Cosmos DB", KeyConcepts = "Multi-model, global distribution, consistency levels, RU/s, partition keys", Tags = new() { "NoSQL", "Global" } },
            new() { Title = "Azure Database for PostgreSQL", Category = "Database", Difficulty = "Medium", AzureService = "PostgreSQL", KeyConcepts = "Managed PostgreSQL, single server vs flexible server, high availability", Tags = new() { "PostgreSQL", "OSS" } },
            new() { Title = "Azure Database for MySQL", Category = "Database", Difficulty = "Medium", AzureService = "MySQL", KeyConcepts = "Managed MySQL, flexible server, read replicas, backup/restore", Tags = new() { "MySQL", "OSS" } },
            new() { Title = "Azure Cache for Redis", Category = "Database", Difficulty = "Medium", AzureService = "Redis Cache", KeyConcepts = "In-memory cache, data structures, persistence, clustering, geo-replication", Tags = new() { "Caching", "Redis" } },
            new() { Title = "Azure SQL Managed Instance", Category = "Database", Difficulty = "Medium", AzureService = "SQL MI", KeyConcepts = "Near 100% SQL Server compatibility, VNet integration, instance pools", Tags = new() { "SQL Server", "Migration" } },

            // Networking
            new() { Title = "Azure Virtual Network (VNet)", Category = "Networking", Difficulty = "Medium", AzureService = "VNet", KeyConcepts = "Subnets, NSGs, route tables, VNet peering, service endpoints", Tags = new() { "VNet", "Networking" } },
            new() { Title = "Azure Load Balancer", Category = "Networking", Difficulty = "Medium", AzureService = "Load Balancer", KeyConcepts = "L4 load balancing, public vs internal, health probes, backend pools", Tags = new() { "Load Balancing" } },
            new() { Title = "Azure Application Gateway", Category = "Networking", Difficulty = "Medium", AzureService = "App Gateway", KeyConcepts = "L7 load balancing, WAF, SSL termination, URL-based routing, autoscaling", Tags = new() { "Load Balancing", "WAF" } },
            new() { Title = "Azure Front Door", Category = "Networking", Difficulty = "Medium", AzureService = "Front Door", KeyConcepts = "Global load balancing, CDN, WAF, SSL offloading, intelligent routing", Tags = new() { "CDN", "Global" } },
            new() { Title = "Azure CDN", Category = "Networking", Difficulty = "Easy", AzureService = "CDN", KeyConcepts = "Content caching, POP locations, caching rules, purge/preload", Tags = new() { "CDN", "Caching" } },
            new() { Title = "Azure DNS", Category = "Networking", Difficulty = "Easy", AzureService = "DNS", KeyConcepts = "DNS zones, record sets, alias records, private DNS zones", Tags = new() { "DNS" } },
            new() { Title = "Azure VPN Gateway", Category = "Networking", Difficulty = "Medium", AzureService = "VPN Gateway", KeyConcepts = "Site-to-site, point-to-site, VNet-to-VNet, BGP, active-active", Tags = new() { "VPN", "Hybrid" } },
            new() { Title = "Azure ExpressRoute", Category = "Networking", Difficulty = "Hard", AzureService = "ExpressRoute", KeyConcepts = "Private connectivity, peering types, circuits, Global Reach", Tags = new() { "Hybrid", "Private" } },
            new() { Title = "Azure Private Link", Category = "Networking", Difficulty = "Medium", AzureService = "Private Link", KeyConcepts = "Private endpoints, private access to PaaS, no public IP needed", Tags = new() { "Security", "Private" } },

            // Security & Identity
            new() { Title = "Azure Active Directory (Entra ID)", Category = "Identity", Difficulty = "Medium", AzureService = "Azure AD", KeyConcepts = "Identity provider, SSO, MFA, Conditional Access, B2B/B2C, App registrations", Tags = new() { "Identity", "IAM" } },
            new() { Title = "Azure Key Vault", Category = "Security", Difficulty = "Medium", AzureService = "Key Vault", KeyConcepts = "Secrets, keys, certificates, HSM, access policies, RBAC", Tags = new() { "Security", "Secrets" } },
            new() { Title = "Azure Security Center / Defender for Cloud", Category = "Security", Difficulty = "Medium", AzureService = "Defender", KeyConcepts = "Security posture, recommendations, secure score, threat protection", Tags = new() { "Security", "Compliance" } },
            new() { Title = "Azure DDoS Protection", Category = "Security", Difficulty = "Easy", AzureService = "DDoS Protection", KeyConcepts = "Basic vs Standard, adaptive tuning, attack analytics", Tags = new() { "Security", "DDoS" } },
            new() { Title = "Azure Firewall", Category = "Security", Difficulty = "Medium", AzureService = "Firewall", KeyConcepts = "Network and application rules, threat intelligence, FQDN filtering", Tags = new() { "Security", "Firewall" } },
            new() { Title = "Managed Identities", Category = "Identity", Difficulty = "Easy", AzureService = "Managed Identity", KeyConcepts = "System-assigned vs user-assigned, passwordless authentication to Azure services", Tags = new() { "Identity", "Security" } },
            new() { Title = "Azure RBAC", Category = "Identity", Difficulty = "Medium", AzureService = "RBAC", KeyConcepts = "Role assignments, built-in roles, custom roles, scope hierarchy", Tags = new() { "IAM", "Authorization" } },

            // Integration & Messaging
            new() { Title = "Azure Service Bus", Category = "Integration", Difficulty = "Medium", AzureService = "Service Bus", KeyConcepts = "Queues, topics/subscriptions, sessions, dead-letter, duplicate detection", Tags = new() { "Messaging", "Enterprise" } },
            new() { Title = "Azure Event Hubs", Category = "Integration", Difficulty = "Medium", AzureService = "Event Hubs", KeyConcepts = "Big data streaming, partitions, consumer groups, Kafka API, capture", Tags = new() { "Streaming", "Big Data" } },
            new() { Title = "Azure Event Grid", Category = "Integration", Difficulty = "Medium", AzureService = "Event Grid", KeyConcepts = "Event-driven architecture, topics, subscriptions, event handlers, filtering", Tags = new() { "Events", "Serverless" } },
            new() { Title = "Azure Logic Apps", Category = "Integration", Difficulty = "Medium", AzureService = "Logic Apps", KeyConcepts = "Workflow automation, connectors, triggers, actions, Enterprise Integration Pack", Tags = new() { "Integration", "Workflow" } },
            new() { Title = "Azure API Management", Category = "Integration", Difficulty = "Medium", AzureService = "APIM", KeyConcepts = "API gateway, policies, products, subscriptions, developer portal", Tags = new() { "API", "Gateway" } },

            // DevOps & Monitoring
            new() { Title = "Azure DevOps Services", Category = "DevOps", Difficulty = "Medium", AzureService = "Azure DevOps", KeyConcepts = "Repos, Pipelines, Boards, Artifacts, Test Plans, YAML pipelines", Tags = new() { "CI/CD", "DevOps" } },
            new() { Title = "Azure Monitor", Category = "Monitoring", Difficulty = "Medium", AzureService = "Monitor", KeyConcepts = "Metrics, logs, alerts, dashboards, Application Insights, Log Analytics", Tags = new() { "Monitoring", "Observability" } },
            new() { Title = "Application Insights", Category = "Monitoring", Difficulty = "Medium", AzureService = "App Insights", KeyConcepts = "APM, telemetry, distributed tracing, live metrics, availability tests", Tags = new() { "APM", "Monitoring" } },
            new() { Title = "Log Analytics", Category = "Monitoring", Difficulty = "Medium", AzureService = "Log Analytics", KeyConcepts = "KQL queries, workspaces, data collection, retention, workbooks", Tags = new() { "Logging", "KQL" } },
            new() { Title = "Azure Resource Manager (ARM)", Category = "DevOps", Difficulty = "Medium", AzureService = "ARM", KeyConcepts = "ARM templates, Bicep, resource groups, deployments, what-if", Tags = new() { "IaC", "Templates" } },
            new() { Title = "Azure Bicep", Category = "DevOps", Difficulty = "Medium", AzureService = "Bicep", KeyConcepts = "DSL for ARM, modules, parameters, outputs, what-if deployments", Tags = new() { "IaC", "Bicep" } },

            // AI & Machine Learning
            new() { Title = "Azure OpenAI Service", Category = "AI", Difficulty = "Medium", AzureService = "OpenAI", KeyConcepts = "GPT models, embeddings, fine-tuning, responsible AI, content filtering", Tags = new() { "AI", "LLM" } },
            new() { Title = "Azure Cognitive Services", Category = "AI", Difficulty = "Medium", AzureService = "Cognitive Services", KeyConcepts = "Vision, Speech, Language, Decision APIs, custom models", Tags = new() { "AI", "Cognitive" } },
            new() { Title = "Azure Machine Learning", Category = "AI", Difficulty = "Hard", AzureService = "Azure ML", KeyConcepts = "Workspaces, pipelines, endpoints, AutoML, MLOps, compute clusters", Tags = new() { "ML", "MLOps" } },

            // Data & Analytics
            new() { Title = "Azure Synapse Analytics", Category = "Analytics", Difficulty = "Hard", AzureService = "Synapse", KeyConcepts = "Data warehousing, Spark pools, SQL pools, pipelines, Power BI integration", Tags = new() { "Analytics", "Data Warehouse" } },
            new() { Title = "Azure Data Factory", Category = "Analytics", Difficulty = "Medium", AzureService = "Data Factory", KeyConcepts = "ETL/ELT, pipelines, data flows, triggers, linked services, integration runtime", Tags = new() { "ETL", "Data Integration" } },
            new() { Title = "Azure Databricks", Category = "Analytics", Difficulty = "Hard", AzureService = "Databricks", KeyConcepts = "Apache Spark, notebooks, clusters, Delta Lake, MLflow", Tags = new() { "Spark", "Big Data" } },
            new() { Title = "Azure Stream Analytics", Category = "Analytics", Difficulty = "Medium", AzureService = "Stream Analytics", KeyConcepts = "Real-time analytics, SQL-like queries, windowing functions, inputs/outputs", Tags = new() { "Streaming", "Real-time" } },

            // Governance & Management
            new() { Title = "Azure Policy", Category = "Governance", Difficulty = "Medium", AzureService = "Policy", KeyConcepts = "Policy definitions, initiatives, compliance, remediation, exemptions", Tags = new() { "Governance", "Compliance" } },
            new() { Title = "Azure Blueprints", Category = "Governance", Difficulty = "Medium", AzureService = "Blueprints", KeyConcepts = "Environment templates, artifacts, versioning, locking, assignment", Tags = new() { "Governance", "Templates" } },
            new() { Title = "Azure Cost Management", Category = "Governance", Difficulty = "Easy", AzureService = "Cost Management", KeyConcepts = "Cost analysis, budgets, alerts, recommendations, exports", Tags = new() { "FinOps", "Cost" } },
            new() { Title = "Azure Management Groups", Category = "Governance", Difficulty = "Easy", AzureService = "Management Groups", KeyConcepts = "Hierarchy, inheritance, policy assignment, RBAC scope", Tags = new() { "Governance", "Organization" } },
        };
    }

    public static List<OOPTopic> GetOOPTopics()
    {
        return new List<OOPTopic>
        {
            // Core Principles (SOLID)
            new() { Title = "Single Responsibility Principle (SRP)", Category = "SOLID Principles", Difficulty = "Easy", KeyConcepts = "A class should have only one reason to change. Each class should do one thing well.", CodeExample = "class UserRepository { Save(); } class UserValidator { Validate(); }", Tags = new() { "SOLID", "Design" } },
            new() { Title = "Open/Closed Principle (OCP)", Category = "SOLID Principles", Difficulty = "Medium", KeyConcepts = "Open for extension, closed for modification. Use abstraction and inheritance.", CodeExample = "abstract class Shape { abstract double Area(); } class Circle : Shape { override Area() }", Tags = new() { "SOLID", "Extensibility" } },
            new() { Title = "Liskov Substitution Principle (LSP)", Category = "SOLID Principles", Difficulty = "Medium", KeyConcepts = "Subtypes must be substitutable for their base types without altering program correctness.", CodeExample = "Bird bird = new Penguin(); bird.Fly(); // Violates LSP if Penguin can't fly", Tags = new() { "SOLID", "Inheritance" } },
            new() { Title = "Interface Segregation Principle (ISP)", Category = "SOLID Principles", Difficulty = "Medium", KeyConcepts = "Clients shouldn't depend on interfaces they don't use. Prefer small, specific interfaces.", CodeExample = "interface IReader { Read(); } interface IWriter { Write(); } // Instead of IReaderWriter", Tags = new() { "SOLID", "Interfaces" } },
            new() { Title = "Dependency Inversion Principle (DIP)", Category = "SOLID Principles", Difficulty = "Medium", KeyConcepts = "High-level modules shouldn't depend on low-level modules. Both should depend on abstractions.", CodeExample = "class OrderService { IRepository _repo; OrderService(IRepository repo) { _repo = repo; } }", Tags = new() { "SOLID", "DI" } },

            // Four Pillars of OOP
            new() { Title = "Encapsulation", Category = "Four Pillars", Difficulty = "Easy", KeyConcepts = "Bundling data and methods, hiding internal state. Access modifiers: public, private, protected, internal.", CodeExample = "class Account { private decimal _balance; public void Deposit(decimal amount) { _balance += amount; } }", Tags = new() { "Fundamentals" } },
            new() { Title = "Abstraction", Category = "Four Pillars", Difficulty = "Easy", KeyConcepts = "Hiding complex implementation details, showing only essential features. Abstract classes and interfaces.", CodeExample = "abstract class Vehicle { abstract void Start(); } // Hides how different vehicles start", Tags = new() { "Fundamentals" } },
            new() { Title = "Inheritance", Category = "Four Pillars", Difficulty = "Easy", KeyConcepts = "Creating new classes from existing ones. IS-A relationship. Base and derived classes.", CodeExample = "class Animal { void Eat(); } class Dog : Animal { void Bark(); }", Tags = new() { "Fundamentals" } },
            new() { Title = "Polymorphism", Category = "Four Pillars", Difficulty = "Medium", KeyConcepts = "Same interface, different implementations. Method overriding, method overloading, virtual methods.", CodeExample = "Animal animal = new Dog(); animal.MakeSound(); // Calls Dog's implementation", Tags = new() { "Fundamentals" } },

            // OOP Concepts
            new() { Title = "Classes and Objects", Category = "Concepts", Difficulty = "Easy", KeyConcepts = "Class is a blueprint, object is an instance. Fields, properties, methods, constructors.", CodeExample = "class Person { string Name; } Person p = new Person();", Tags = new() { "Basics" } },
            new() { Title = "Constructors", Category = "Concepts", Difficulty = "Easy", KeyConcepts = "Default, parameterized, copy constructors. Constructor chaining, static constructors.", CodeExample = "class Car { public Car() { } public Car(string model) : this() { } }", Tags = new() { "Basics" } },
            new() { Title = "Access Modifiers", Category = "Concepts", Difficulty = "Easy", KeyConcepts = "public, private, protected, internal, protected internal, private protected", CodeExample = "public class A { private int x; protected int y; internal int z; }", Tags = new() { "Visibility" } },
            new() { Title = "Properties", Category = "Concepts", Difficulty = "Easy", KeyConcepts = "Getters and setters, auto-properties, computed properties, init-only setters.", CodeExample = "public string Name { get; set; } public int Age { get; private set; }", Tags = new() { "C#" } },
            new() { Title = "Static Members", Category = "Concepts", Difficulty = "Easy", KeyConcepts = "Class-level members, shared across instances. Static classes, static constructors.", CodeExample = "static class Math { public static int Add(int a, int b) => a + b; }", Tags = new() { "Basics" } },
            new() { Title = "Abstract Classes", Category = "Concepts", Difficulty = "Medium", KeyConcepts = "Cannot be instantiated, can have abstract and concrete members. Template for derived classes.", CodeExample = "abstract class Shape { abstract double Area(); void Display() { } }", Tags = new() { "Abstraction" } },
            new() { Title = "Interfaces", Category = "Concepts", Difficulty = "Medium", KeyConcepts = "Contract definition, multiple inheritance, default interface methods (C# 8+).", CodeExample = "interface IDisposable { void Dispose(); } class Resource : IDisposable { }", Tags = new() { "Contracts" } },
            new() { Title = "Sealed Classes", Category = "Concepts", Difficulty = "Easy", KeyConcepts = "Cannot be inherited. Performance benefits, security, design intent.", CodeExample = "sealed class FinalClass { } // Cannot inherit from this", Tags = new() { "Inheritance" } },
            new() { Title = "Partial Classes", Category = "Concepts", Difficulty = "Easy", KeyConcepts = "Split class definition across files. Used in code generation, separation of concerns.", CodeExample = "partial class Employee { } // In file 1\npartial class Employee { } // In file 2", Tags = new() { "C#" } },

            // Relationships
            new() { Title = "Association", Category = "Relationships", Difficulty = "Easy", KeyConcepts = "General relationship between objects. HAS-A relationship. Can be bidirectional.", CodeExample = "class Teacher { List<Student> Students; } class Student { Teacher Mentor; }", Tags = new() { "Design" } },
            new() { Title = "Aggregation", Category = "Relationships", Difficulty = "Easy", KeyConcepts = "Weak HAS-A relationship. Parts can exist independently. Whole-part relationship.", CodeExample = "class Department { List<Employee> Employees; } // Employees exist without Department", Tags = new() { "Design" } },
            new() { Title = "Composition", Category = "Relationships", Difficulty = "Medium", KeyConcepts = "Strong HAS-A relationship. Parts cannot exist without whole. Lifetime dependency.", CodeExample = "class House { Room room = new Room(); } // Room dies with House", Tags = new() { "Design" } },
            new() { Title = "Dependency", Category = "Relationships", Difficulty = "Easy", KeyConcepts = "USES-A relationship. One class uses another temporarily. Method parameter or local variable.", CodeExample = "class OrderProcessor { void Process(IPaymentGateway gateway) { } }", Tags = new() { "Design" } },

            // Advanced Concepts
            new() { Title = "Method Overloading", Category = "Advanced", Difficulty = "Easy", KeyConcepts = "Same method name, different parameters. Compile-time polymorphism.", CodeExample = "void Print(int x) { } void Print(string s) { } void Print(int x, int y) { }", Tags = new() { "Polymorphism" } },
            new() { Title = "Method Overriding", Category = "Advanced", Difficulty = "Medium", KeyConcepts = "Redefine base class method. virtual/override keywords. Runtime polymorphism.", CodeExample = "class Base { virtual void Show() { } } class Derived : Base { override void Show() { } }", Tags = new() { "Polymorphism" } },
            new() { Title = "Method Hiding (new keyword)", Category = "Advanced", Difficulty = "Medium", KeyConcepts = "Hide base class method with new keyword. Different from overriding.", CodeExample = "class Derived : Base { new void Show() { } } // Hides, doesn't override", Tags = new() { "Inheritance" } },
            new() { Title = "Covariance and Contravariance", Category = "Advanced", Difficulty = "Hard", KeyConcepts = "Type parameter variance in generics. out = covariant, in = contravariant.", CodeExample = "IEnumerable<Derived> d; IEnumerable<Base> b = d; // Covariance", Tags = new() { "Generics" } },
            new() { Title = "Object Cloning", Category = "Advanced", Difficulty = "Medium", KeyConcepts = "Shallow vs deep copy. ICloneable interface. MemberwiseClone method.", CodeExample = "class Person : ICloneable { public object Clone() => MemberwiseClone(); }", Tags = new() { "Copying" } },
            new() { Title = "Operator Overloading", Category = "Advanced", Difficulty = "Medium", KeyConcepts = "Custom operators for classes. +, -, ==, !=, implicit/explicit conversions.", CodeExample = "public static Money operator +(Money a, Money b) => new Money(a.Amount + b.Amount);", Tags = new() { "Operators" } },

            // Design Principles
            new() { Title = "Composition over Inheritance", Category = "Design Principles", Difficulty = "Medium", KeyConcepts = "Prefer object composition to class inheritance. More flexible, less coupling.", CodeExample = "class Car { Engine engine; } // Instead of class Car : Engine", Tags = new() { "Design", "Best Practice" } },
            new() { Title = "Program to Interface, Not Implementation", Category = "Design Principles", Difficulty = "Medium", KeyConcepts = "Depend on abstractions, not concrete classes. Enables flexibility and testing.", CodeExample = "ILogger logger = new FileLogger(); // Not: FileLogger logger = new FileLogger();", Tags = new() { "Design", "DI" } },
            new() { Title = "Law of Demeter", Category = "Design Principles", Difficulty = "Medium", KeyConcepts = "Only talk to immediate friends. Minimize coupling. Avoid method chaining.", CodeExample = "// Bad: obj.GetA().GetB().GetC()\n// Good: obj.DoSomething()", Tags = new() { "Design", "Coupling" } },
            new() { Title = "Tell, Don't Ask", Category = "Design Principles", Difficulty = "Medium", KeyConcepts = "Tell objects what to do, don't ask for data and act on it. Encapsulation.", CodeExample = "// Bad: if (user.IsAdmin()) DoAdminStuff();\n// Good: user.PerformAdminAction()", Tags = new() { "Design", "Encapsulation" } },
            new() { Title = "DRY (Don't Repeat Yourself)", Category = "Design Principles", Difficulty = "Easy", KeyConcepts = "Every piece of knowledge must have a single representation. Avoid duplication.", CodeExample = "// Extract common logic into reusable methods/classes", Tags = new() { "Best Practice" } },
            new() { Title = "KISS (Keep It Simple, Stupid)", Category = "Design Principles", Difficulty = "Easy", KeyConcepts = "Simplicity should be a key goal. Avoid unnecessary complexity.", CodeExample = "// Choose the simplest solution that works", Tags = new() { "Best Practice" } },
            new() { Title = "YAGNI (You Aren't Gonna Need It)", Category = "Design Principles", Difficulty = "Easy", KeyConcepts = "Don't implement features until they're actually needed. Avoid over-engineering.", CodeExample = "// Don't add extensibility points 'just in case'", Tags = new() { "Best Practice" } },
        };
    }

    public static List<CSharpTopic> GetCSharpTopics()
    {
        return new List<CSharpTopic>
        {
            // Fundamentals
            new() { Title = "Value Types vs Reference Types", Category = "Fundamentals", Difficulty = "Easy", KeyConcepts = "Stack vs heap allocation, struct vs class, boxing/unboxing, nullable value types", DotNetVersion = "1.0", Tags = new() { "Types", "Memory" } },
            new() { Title = "String Manipulation", Category = "Fundamentals", Difficulty = "Easy", KeyConcepts = "Immutability, StringBuilder, string interpolation, verbatim strings, raw string literals", DotNetVersion = "1.0", Tags = new() { "Strings" } },
            new() { Title = "Nullable Types", Category = "Fundamentals", Difficulty = "Easy", KeyConcepts = "Nullable<T>, ?, null-coalescing (??, ??=), null-conditional (?.), nullable reference types", DotNetVersion = "2.0", Tags = new() { "Null Safety" } },
            new() { Title = "Tuples", Category = "Fundamentals", Difficulty = "Easy", KeyConcepts = "ValueTuple, named elements, deconstruction, tuple equality", DotNetVersion = "7.0", Tags = new() { "Data Structures" } },
            new() { Title = "Records", Category = "Fundamentals", Difficulty = "Medium", KeyConcepts = "Immutable by default, value equality, with expressions, record struct (C# 10)", DotNetVersion = "9.0", Tags = new() { "Types", "Immutability" } },
            new() { Title = "Pattern Matching", Category = "Fundamentals", Difficulty = "Medium", KeyConcepts = "Type patterns, property patterns, switch expressions, relational patterns, list patterns", DotNetVersion = "7.0", Tags = new() { "Modern C#" } },

            // Collections & LINQ
            new() { Title = "Arrays and Collections", Category = "Collections", Difficulty = "Easy", KeyConcepts = "Array, List<T>, Dictionary<K,V>, HashSet<T>, Queue<T>, Stack<T>, LinkedList<T>", DotNetVersion = "1.0", Tags = new() { "Collections" } },
            new() { Title = "IEnumerable and IEnumerator", Category = "Collections", Difficulty = "Medium", KeyConcepts = "Iteration protocol, yield return, deferred execution, foreach mechanics", DotNetVersion = "1.0", Tags = new() { "Collections", "Iteration" } },
            new() { Title = "LINQ Basics", Category = "LINQ", Difficulty = "Medium", KeyConcepts = "Query syntax vs method syntax, Where, Select, OrderBy, GroupBy, Join", DotNetVersion = "3.5", Tags = new() { "LINQ", "Querying" } },
            new() { Title = "LINQ Advanced", Category = "LINQ", Difficulty = "Hard", KeyConcepts = "Aggregate, SelectMany, custom comparers, expression trees, IQueryable vs IEnumerable", DotNetVersion = "3.5", Tags = new() { "LINQ", "Advanced" } },
            new() { Title = "Span<T> and Memory<T>", Category = "Collections", Difficulty = "Hard", KeyConcepts = "Stack-only types, slicing without allocation, ReadOnlySpan, ArrayPool", DotNetVersion = "7.2", Tags = new() { "Performance", "Memory" } },

            // Asynchronous Programming
            new() { Title = "async/await Fundamentals", Category = "Async", Difficulty = "Medium", KeyConcepts = "Task, Task<T>, async methods, await keyword, ConfigureAwait, ValueTask", DotNetVersion = "5.0", Tags = new() { "Async", "TPL" } },
            new() { Title = "Task Parallel Library (TPL)", Category = "Async", Difficulty = "Medium", KeyConcepts = "Task.Run, Task.WhenAll, Task.WhenAny, Parallel.For, Parallel.ForEach", DotNetVersion = "4.0", Tags = new() { "Parallelism", "Threading" } },
            new() { Title = "Cancellation Tokens", Category = "Async", Difficulty = "Medium", KeyConcepts = "CancellationTokenSource, cooperative cancellation, linked tokens, timeouts", DotNetVersion = "4.0", Tags = new() { "Async", "Cancellation" } },
            new() { Title = "IAsyncEnumerable", Category = "Async", Difficulty = "Medium", KeyConcepts = "Async streams, await foreach, async yield return", DotNetVersion = "8.0", Tags = new() { "Async", "Streaming" } },
            new() { Title = "Channels", Category = "Async", Difficulty = "Hard", KeyConcepts = "Producer-consumer, bounded/unbounded, Channel<T>.CreateBounded/Unbounded", DotNetVersion = "Core 3.0", Tags = new() { "Async", "Concurrency" } },
            new() { Title = "Thread Safety", Category = "Async", Difficulty = "Hard", KeyConcepts = "lock, Monitor, Mutex, Semaphore, ReaderWriterLockSlim, Interlocked, volatile", DotNetVersion = "1.0", Tags = new() { "Threading", "Concurrency" } },

            // Memory Management
            new() { Title = "Garbage Collection", Category = "Memory", Difficulty = "Medium", KeyConcepts = "Generations (0, 1, 2), LOH, GC modes (workstation/server), GC.Collect, finalizers", DotNetVersion = "1.0", Tags = new() { "Memory", "GC" } },
            new() { Title = "IDisposable Pattern", Category = "Memory", Difficulty = "Medium", KeyConcepts = "using statement, Dispose pattern, SafeHandle, GC.SuppressFinalize", DotNetVersion = "1.0", Tags = new() { "Memory", "Resources" } },
            new() { Title = "ref, in, out Keywords", Category = "Memory", Difficulty = "Medium", KeyConcepts = "Pass by reference, ref locals, ref returns, in (readonly ref), ref struct", DotNetVersion = "7.2", Tags = new() { "Parameters", "Performance" } },
            new() { Title = "stackalloc", Category = "Memory", Difficulty = "Hard", KeyConcepts = "Stack allocation, Span<T> with stackalloc, avoiding heap allocations", DotNetVersion = "7.2", Tags = new() { "Performance", "Memory" } },

            // Delegates & Events
            new() { Title = "Delegates", Category = "Delegates", Difficulty = "Medium", KeyConcepts = "Function pointers, multicast delegates, Func<T>, Action<T>, Predicate<T>", DotNetVersion = "1.0", Tags = new() { "Delegates" } },
            new() { Title = "Lambda Expressions", Category = "Delegates", Difficulty = "Easy", KeyConcepts = "Expression lambdas, statement lambdas, closures, static lambdas", DotNetVersion = "3.0", Tags = new() { "Lambdas", "Functional" } },
            new() { Title = "Events", Category = "Delegates", Difficulty = "Medium", KeyConcepts = "event keyword, EventHandler<T>, add/remove accessors, weak events", DotNetVersion = "1.0", Tags = new() { "Events", "Pub-Sub" } },
            new() { Title = "Expression Trees", Category = "Delegates", Difficulty = "Hard", KeyConcepts = "Expression<Func<T>>, building expressions, IQueryable, dynamic compilation", DotNetVersion = "3.5", Tags = new() { "Metaprogramming", "LINQ" } },

            // Generics
            new() { Title = "Generic Types", Category = "Generics", Difficulty = "Medium", KeyConcepts = "Type parameters, generic classes, generic methods, generic interfaces", DotNetVersion = "2.0", Tags = new() { "Generics" } },
            new() { Title = "Generic Constraints", Category = "Generics", Difficulty = "Medium", KeyConcepts = "where T : class/struct/new()/IInterface/BaseClass, notnull, unmanaged", DotNetVersion = "2.0", Tags = new() { "Generics", "Constraints" } },
            new() { Title = "Covariance & Contravariance", Category = "Generics", Difficulty = "Hard", KeyConcepts = "out (covariant), in (contravariant), IEnumerable<out T>, Action<in T>", DotNetVersion = "4.0", Tags = new() { "Generics", "Variance" } },

            // Modern C# Features
            new() { Title = "Source Generators", Category = "Modern C#", Difficulty = "Hard", KeyConcepts = "Compile-time code generation, ISourceGenerator, incremental generators", DotNetVersion = "9.0", Tags = new() { "Roslyn", "Metaprogramming" } },
            new() { Title = "Attributes & Reflection", Category = "Modern C#", Difficulty = "Medium", KeyConcepts = "Custom attributes, Type reflection, MethodInfo, PropertyInfo, GetCustomAttributes", DotNetVersion = "1.0", Tags = new() { "Metadata", "Reflection" } },
            new() { Title = "Primary Constructors", Category = "Modern C#", Difficulty = "Easy", KeyConcepts = "Constructor parameters as fields, record-like syntax for classes", DotNetVersion = "12.0", Tags = new() { "C# 12" } },
            new() { Title = "Collection Expressions", Category = "Modern C#", Difficulty = "Easy", KeyConcepts = "[ ] syntax, spread operator (..), target-typed collections", DotNetVersion = "12.0", Tags = new() { "C# 12" } },
            new() { Title = "Required Members", Category = "Modern C#", Difficulty = "Easy", KeyConcepts = "required keyword, SetsRequiredMembers attribute", DotNetVersion = "11.0", Tags = new() { "C# 11" } },
            new() { Title = "File-scoped Types", Category = "Modern C#", Difficulty = "Easy", KeyConcepts = "file access modifier, internal to single file", DotNetVersion = "11.0", Tags = new() { "C# 11" } },
            new() { Title = "Raw String Literals", Category = "Modern C#", Difficulty = "Easy", KeyConcepts = "\"\"\" syntax, multiline strings, interpolation with $\"\"\"", DotNetVersion = "11.0", Tags = new() { "C# 11", "Strings" } },
            new() { Title = "init Accessor", Category = "Modern C#", Difficulty = "Easy", KeyConcepts = "Immutable properties settable at initialization, init keyword", DotNetVersion = "9.0", Tags = new() { "C# 9", "Immutability" } },

            // Error Handling
            new() { Title = "Exception Handling", Category = "Error Handling", Difficulty = "Easy", KeyConcepts = "try-catch-finally, exception filters (when), throw expressions, custom exceptions", DotNetVersion = "1.0", Tags = new() { "Exceptions" } },
            new() { Title = "Exception Best Practices", Category = "Error Handling", Difficulty = "Medium", KeyConcepts = "Don't catch Exception, preserve stack trace, ExceptionDispatchInfo, AggregateException", DotNetVersion = "1.0", Tags = new() { "Best Practices" } },

            // Interoperability
            new() { Title = "P/Invoke", Category = "Interop", Difficulty = "Hard", KeyConcepts = "DllImport, marshalling, LibraryImport (source-generated), native interop", DotNetVersion = "1.0", Tags = new() { "Native", "Interop" } },
            new() { Title = "COM Interop", Category = "Interop", Difficulty = "Hard", KeyConcepts = "ComImport, runtime callable wrappers, dynamic keyword with COM", DotNetVersion = "1.0", Tags = new() { "COM", "Interop" } },
        };
    }

    public static List<AspNetCoreTopic> GetAspNetCoreTopics()
    {
        return new List<AspNetCoreTopic>
        {
            // Fundamentals
            new() { Title = "ASP.NET Core Pipeline", Category = "Fundamentals", Difficulty = "Medium", KeyConcepts = "Request pipeline, middleware ordering, terminal middleware, branching (Map, MapWhen)", Tags = new() { "Pipeline", "Architecture" } },
            new() { Title = "Middleware", Category = "Fundamentals", Difficulty = "Medium", KeyConcepts = "Use, Run, Map, custom middleware, IMiddleware interface, order matters", Tags = new() { "Middleware", "Pipeline" } },
            new() { Title = "Configuration", Category = "Fundamentals", Difficulty = "Easy", KeyConcepts = "appsettings.json, environment variables, IConfiguration, Options pattern, secrets", Tags = new() { "Configuration" } },
            new() { Title = "Logging", Category = "Fundamentals", Difficulty = "Easy", KeyConcepts = "ILogger<T>, log levels, providers (Console, Debug, File), structured logging, Serilog", Tags = new() { "Logging", "Observability" } },
            new() { Title = "Environments", Category = "Fundamentals", Difficulty = "Easy", KeyConcepts = "Development, Staging, Production, ASPNETCORE_ENVIRONMENT, environment-specific config", Tags = new() { "Configuration" } },

            // Dependency Injection
            new() { Title = "DI Container", Category = "Dependency Injection", Difficulty = "Medium", KeyConcepts = "Service registration, Transient/Scoped/Singleton lifetimes, IServiceCollection", Tags = new() { "DI", "IoC" } },
            new() { Title = "Service Lifetimes", Category = "Dependency Injection", Difficulty = "Medium", KeyConcepts = "Transient (new each time), Scoped (per request), Singleton (app lifetime), captive dependencies", Tags = new() { "DI", "Lifetimes" } },
            new() { Title = "Advanced DI Patterns", Category = "Dependency Injection", Difficulty = "Hard", KeyConcepts = "Factory pattern, keyed services, IServiceScopeFactory, open generics, decorators", Tags = new() { "DI", "Patterns" } },

            // Web API
            new() { Title = "Controllers & Actions", Category = "Web API", Difficulty = "Easy", KeyConcepts = "ControllerBase, ApiController attribute, action methods, routing, return types", Tags = new() { "API", "Controllers" } },
            new() { Title = "Routing", Category = "Web API", Difficulty = "Medium", KeyConcepts = "Attribute routing, conventional routing, route constraints, route templates, areas", Tags = new() { "Routing" } },
            new() { Title = "Model Binding", Category = "Web API", Difficulty = "Medium", KeyConcepts = "[FromBody], [FromQuery], [FromRoute], [FromHeader], [FromForm], custom binders", Tags = new() { "Binding" } },
            new() { Title = "Validation", Category = "Web API", Difficulty = "Medium", KeyConcepts = "Data annotations, FluentValidation, ModelState, IValidatableObject, custom validators", Tags = new() { "Validation" } },
            new() { Title = "Action Results", Category = "Web API", Difficulty = "Easy", KeyConcepts = "IActionResult, ActionResult<T>, Ok, BadRequest, NotFound, Created, Problem Details", Tags = new() { "API", "Responses" } },
            new() { Title = "Content Negotiation", Category = "Web API", Difficulty = "Medium", KeyConcepts = "Accept header, formatters, JSON/XML serialization, custom formatters", Tags = new() { "API", "Serialization" } },
            new() { Title = "Minimal APIs", Category = "Web API", Difficulty = "Medium", KeyConcepts = "MapGet/Post/Put/Delete, endpoint filters, route groups, TypedResults", Tags = new() { "Minimal API", "Modern" } },

            // Security
            new() { Title = "Authentication", Category = "Security", Difficulty = "Medium", KeyConcepts = "Cookie auth, JWT Bearer, OAuth2/OIDC, Identity, external providers", Tags = new() { "Auth", "Security" } },
            new() { Title = "Authorization", Category = "Security", Difficulty = "Medium", KeyConcepts = "[Authorize], policies, roles, claims, requirements, resource-based auth", Tags = new() { "Auth", "Security" } },
            new() { Title = "JWT Authentication", Category = "Security", Difficulty = "Medium", KeyConcepts = "Token generation, validation, refresh tokens, claims, JwtSecurityTokenHandler", Tags = new() { "JWT", "Tokens" } },
            new() { Title = "ASP.NET Core Identity", Category = "Security", Difficulty = "Medium", KeyConcepts = "UserManager, SignInManager, roles, claims, password hashing, 2FA", Tags = new() { "Identity", "Users" } },
            new() { Title = "CORS", Category = "Security", Difficulty = "Easy", KeyConcepts = "Cross-origin requests, policies, WithOrigins, AllowAnyOrigin, credentials", Tags = new() { "CORS", "Security" } },
            new() { Title = "Data Protection", Category = "Security", Difficulty = "Medium", KeyConcepts = "IDataProtector, key management, purpose strings, cookie encryption", Tags = new() { "Encryption", "Security" } },
            new() { Title = "HTTPS & Security Headers", Category = "Security", Difficulty = "Easy", KeyConcepts = "HSTS, UseHttpsRedirection, CSP, X-Content-Type-Options, X-Frame-Options", Tags = new() { "Security", "Headers" } },

            // Data Access
            new() { Title = "Entity Framework Core Basics", Category = "Data Access", Difficulty = "Medium", KeyConcepts = "DbContext, DbSet, migrations, LINQ queries, change tracking", Tags = new() { "EF Core", "ORM" } },
            new() { Title = "EF Core Relationships", Category = "Data Access", Difficulty = "Medium", KeyConcepts = "One-to-one, one-to-many, many-to-many, navigation properties, Fluent API", Tags = new() { "EF Core", "Relationships" } },
            new() { Title = "EF Core Performance", Category = "Data Access", Difficulty = "Hard", KeyConcepts = "Eager/lazy/explicit loading, no-tracking queries, compiled queries, split queries", Tags = new() { "EF Core", "Performance" } },
            new() { Title = "Dapper", Category = "Data Access", Difficulty = "Medium", KeyConcepts = "Micro-ORM, raw SQL, multi-mapping, stored procedures, performance", Tags = new() { "Dapper", "Performance" } },
            new() { Title = "Repository Pattern", Category = "Data Access", Difficulty = "Medium", KeyConcepts = "Abstraction over data access, Unit of Work, generic repository", Tags = new() { "Patterns", "Architecture" } },

            // Performance
            new() { Title = "Caching", Category = "Performance", Difficulty = "Medium", KeyConcepts = "In-memory cache, distributed cache (Redis), response caching, cache tags", Tags = new() { "Caching", "Performance" } },
            new() { Title = "Response Compression", Category = "Performance", Difficulty = "Easy", KeyConcepts = "Gzip, Brotli, compression middleware, static file compression", Tags = new() { "Performance", "Compression" } },
            new() { Title = "Rate Limiting", Category = "Performance", Difficulty = "Medium", KeyConcepts = "Fixed window, sliding window, token bucket, concurrency limiter", Tags = new() { "Security", "Performance" } },
            new() { Title = "Output Caching", Category = "Performance", Difficulty = "Medium", KeyConcepts = "Endpoint caching, cache policies, vary by, cache tags, invalidation", Tags = new() { "Caching", "Performance" } },

            // Real-time Communication
            new() { Title = "SignalR", Category = "Real-time", Difficulty = "Medium", KeyConcepts = "Hubs, clients, groups, streaming, strongly-typed hubs, scaling", Tags = new() { "WebSocket", "Real-time" } },
            new() { Title = "WebSockets", Category = "Real-time", Difficulty = "Medium", KeyConcepts = "Raw WebSocket support, connection management, message handling", Tags = new() { "WebSocket" } },

            // Testing
            new() { Title = "Unit Testing", Category = "Testing", Difficulty = "Medium", KeyConcepts = "xUnit, NUnit, mocking (Moq, NSubstitute), test organization, AAA pattern", Tags = new() { "Testing", "Unit Tests" } },
            new() { Title = "Integration Testing", Category = "Testing", Difficulty = "Medium", KeyConcepts = "WebApplicationFactory, TestServer, in-memory database, test fixtures", Tags = new() { "Testing", "Integration" } },

            // Architecture
            new() { Title = "Clean Architecture", Category = "Architecture", Difficulty = "Hard", KeyConcepts = "Domain, Application, Infrastructure layers, dependency rule, use cases", Tags = new() { "Architecture", "Design" } },
            new() { Title = "CQRS Pattern", Category = "Architecture", Difficulty = "Hard", KeyConcepts = "Command Query Responsibility Segregation, MediatR, read/write models", Tags = new() { "Patterns", "Architecture" } },
            new() { Title = "MediatR", Category = "Architecture", Difficulty = "Medium", KeyConcepts = "Mediator pattern, requests, handlers, notifications, pipelines", Tags = new() { "Patterns", "Decoupling" } },
            new() { Title = "Health Checks", Category = "Architecture", Difficulty = "Easy", KeyConcepts = "IHealthCheck, database checks, readiness vs liveness, health check UI", Tags = new() { "Monitoring", "DevOps" } },
            new() { Title = "Background Services", Category = "Architecture", Difficulty = "Medium", KeyConcepts = "IHostedService, BackgroundService, scoped services, Hangfire, Quartz", Tags = new() { "Background Jobs" } },

            // OpenAPI & Documentation
            new() { Title = "OpenAPI/Swagger", Category = "Documentation", Difficulty = "Easy", KeyConcepts = "Swashbuckle, NSwag, XML comments, operation filters, document filters", Tags = new() { "API", "Documentation" } },

            // gRPC
            new() { Title = "gRPC Services", Category = "gRPC", Difficulty = "Medium", KeyConcepts = "Protocol buffers, unary/streaming, gRPC-Web, deadlines, interceptors", Tags = new() { "gRPC", "API" } },

            // Deployment
            new() { Title = "Deployment Options", Category = "Deployment", Difficulty = "Medium", KeyConcepts = "Kestrel, IIS, Docker, Azure App Service, self-contained vs framework-dependent", Tags = new() { "Deployment" } },
            new() { Title = "Docker Support", Category = "Deployment", Difficulty = "Medium", KeyConcepts = "Multi-stage builds, .NET images, docker-compose, container optimization", Tags = new() { "Docker", "Containers" } },
        };
    }

    public static List<SqlServerTopic> GetSqlServerTopics()
    {
        return new List<SqlServerTopic>
        {
            // Query Fundamentals
            new() { Title = "SELECT Fundamentals", Category = "Queries", Difficulty = "Easy", KeyConcepts = "SELECT, FROM, WHERE, ORDER BY, DISTINCT, TOP, OFFSET-FETCH, aliases", SqlExample = "SELECT TOP 10 FirstName, LastName FROM Employees WHERE DepartmentId = 1 ORDER BY LastName", Tags = new() { "Basics", "SELECT" } },
            new() { Title = "JOINs", Category = "Queries", Difficulty = "Medium", KeyConcepts = "INNER JOIN, LEFT/RIGHT OUTER JOIN, FULL OUTER JOIN, CROSS JOIN, self-joins", SqlExample = "SELECT e.Name, d.Name FROM Employees e INNER JOIN Departments d ON e.DeptId = d.Id", Tags = new() { "JOINs" } },
            new() { Title = "Subqueries", Category = "Queries", Difficulty = "Medium", KeyConcepts = "Scalar subqueries, correlated subqueries, EXISTS, IN, derived tables", SqlExample = "SELECT * FROM Employees WHERE Salary > (SELECT AVG(Salary) FROM Employees)", Tags = new() { "Subqueries" } },
            new() { Title = "Common Table Expressions (CTEs)", Category = "Queries", Difficulty = "Medium", KeyConcepts = "WITH clause, recursive CTEs, multiple CTEs, readability improvement", SqlExample = "WITH ManagerCTE AS (SELECT * FROM Employees WHERE ManagerId IS NULL) SELECT * FROM ManagerCTE", Tags = new() { "CTE", "Readability" } },
            new() { Title = "Aggregate Functions", Category = "Queries", Difficulty = "Easy", KeyConcepts = "COUNT, SUM, AVG, MIN, MAX, GROUP BY, HAVING, GROUPING SETS, ROLLUP, CUBE", SqlExample = "SELECT DeptId, COUNT(*) as EmpCount FROM Employees GROUP BY DeptId HAVING COUNT(*) > 5", Tags = new() { "Aggregation" } },
            new() { Title = "Window Functions", Category = "Queries", Difficulty = "Hard", KeyConcepts = "ROW_NUMBER, RANK, DENSE_RANK, NTILE, LAG, LEAD, OVER clause, PARTITION BY", SqlExample = "SELECT Name, Salary, ROW_NUMBER() OVER (PARTITION BY DeptId ORDER BY Salary DESC) as Rank FROM Employees", Tags = new() { "Window Functions", "Analytics" } },
            new() { Title = "PIVOT and UNPIVOT", Category = "Queries", Difficulty = "Hard", KeyConcepts = "Transforming rows to columns (PIVOT), columns to rows (UNPIVOT), dynamic pivot", SqlExample = "SELECT * FROM Sales PIVOT (SUM(Amount) FOR Quarter IN ([Q1], [Q2], [Q3], [Q4])) AS PivotTable", Tags = new() { "Transformation" } },

            // Data Manipulation
            new() { Title = "INSERT Statements", Category = "DML", Difficulty = "Easy", KeyConcepts = "INSERT INTO VALUES, INSERT INTO SELECT, bulk insert, OUTPUT clause", SqlExample = "INSERT INTO Employees (Name, DeptId) OUTPUT inserted.Id VALUES ('John', 1)", Tags = new() { "DML", "INSERT" } },
            new() { Title = "UPDATE Statements", Category = "DML", Difficulty = "Easy", KeyConcepts = "UPDATE SET, UPDATE with JOIN, OUTPUT clause, conditional updates", SqlExample = "UPDATE e SET e.Salary = e.Salary * 1.1 FROM Employees e JOIN Departments d ON e.DeptId = d.Id WHERE d.Name = 'IT'", Tags = new() { "DML", "UPDATE" } },
            new() { Title = "DELETE and TRUNCATE", Category = "DML", Difficulty = "Easy", KeyConcepts = "DELETE vs TRUNCATE, DELETE with JOIN, soft delete pattern, OUTPUT clause", SqlExample = "DELETE FROM Employees OUTPUT deleted.* WHERE DepartmentId = 5", Tags = new() { "DML", "DELETE" } },
            new() { Title = "MERGE Statement", Category = "DML", Difficulty = "Medium", KeyConcepts = "Upsert pattern, WHEN MATCHED, WHEN NOT MATCHED, OUTPUT clause", SqlExample = "MERGE Target USING Source ON Target.Id = Source.Id WHEN MATCHED THEN UPDATE... WHEN NOT MATCHED THEN INSERT...", Tags = new() { "DML", "UPSERT" } },

            // Indexes
            new() { Title = "Index Fundamentals", Category = "Indexes", Difficulty = "Medium", KeyConcepts = "Clustered vs non-clustered, B-tree structure, heap tables, index key columns", Tags = new() { "Indexes", "Performance" } },
            new() { Title = "Covering Indexes", Category = "Indexes", Difficulty = "Medium", KeyConcepts = "INCLUDE columns, key lookup elimination, index-only scans", SqlExample = "CREATE INDEX IX_Emp_Dept ON Employees(DepartmentId) INCLUDE (FirstName, LastName)", Tags = new() { "Indexes", "Performance" } },
            new() { Title = "Filtered Indexes", Category = "Indexes", Difficulty = "Medium", KeyConcepts = "Partial indexes, WHERE clause in index, sparse columns", SqlExample = "CREATE INDEX IX_ActiveEmps ON Employees(LastName) WHERE IsActive = 1", Tags = new() { "Indexes" } },
            new() { Title = "Index Maintenance", Category = "Indexes", Difficulty = "Medium", KeyConcepts = "Fragmentation, REORGANIZE vs REBUILD, fill factor, statistics", SqlExample = "ALTER INDEX ALL ON Employees REBUILD WITH (FILLFACTOR = 80)", Tags = new() { "Maintenance" } },
            new() { Title = "Columnstore Indexes", Category = "Indexes", Difficulty = "Hard", KeyConcepts = "Column-based storage, batch processing, data warehousing, compression", Tags = new() { "Analytics", "Data Warehouse" } },

            // Performance
            new() { Title = "Execution Plans", Category = "Performance", Difficulty = "Medium", KeyConcepts = "Estimated vs actual plans, operators, cost percentages, plan cache", Tags = new() { "Performance", "Tuning" } },
            new() { Title = "Query Optimization", Category = "Performance", Difficulty = "Hard", KeyConcepts = "SARGability, parameter sniffing, cardinality estimation, hints", SqlExample = "SELECT * FROM Employees WITH (INDEX(IX_Emp_Name)) WHERE LastName LIKE 'Smith%'", Tags = new() { "Performance", "Optimization" } },
            new() { Title = "Statistics", Category = "Performance", Difficulty = "Medium", KeyConcepts = "Auto-create, auto-update, histogram, density, UPDATE STATISTICS", SqlExample = "UPDATE STATISTICS Employees WITH FULLSCAN", Tags = new() { "Statistics", "Performance" } },
            new() { Title = "Query Store", Category = "Performance", Difficulty = "Medium", KeyConcepts = "Plan history, regressed queries, forcing plans, wait statistics", Tags = new() { "Performance", "Monitoring" } },
            new() { Title = "Deadlocks", Category = "Performance", Difficulty = "Hard", KeyConcepts = "Deadlock detection, deadlock graphs, prevention strategies, retry logic", Tags = new() { "Concurrency", "Locking" } },

            // Stored Procedures & Functions
            new() { Title = "Stored Procedures", Category = "Programmability", Difficulty = "Medium", KeyConcepts = "CREATE PROCEDURE, parameters, OUTPUT parameters, EXEC, sp_executesql", SqlExample = "CREATE PROCEDURE GetEmployees @DeptId INT AS SELECT * FROM Employees WHERE DeptId = @DeptId", Tags = new() { "Stored Procedures" } },
            new() { Title = "User-Defined Functions", Category = "Programmability", Difficulty = "Medium", KeyConcepts = "Scalar functions, inline TVF, multi-statement TVF, determinism", SqlExample = "CREATE FUNCTION GetFullName(@First NVARCHAR(50), @Last NVARCHAR(50)) RETURNS NVARCHAR(100) AS BEGIN RETURN @First + ' ' + @Last END", Tags = new() { "Functions" } },
            new() { Title = "Triggers", Category = "Programmability", Difficulty = "Medium", KeyConcepts = "AFTER/INSTEAD OF triggers, INSERTED/DELETED tables, DML triggers, DDL triggers", SqlExample = "CREATE TRIGGER trg_AuditEmployees ON Employees AFTER INSERT, UPDATE AS INSERT INTO AuditLog SELECT * FROM inserted", Tags = new() { "Triggers", "Automation" } },
            new() { Title = "Dynamic SQL", Category = "Programmability", Difficulty = "Hard", KeyConcepts = "EXEC, sp_executesql, SQL injection prevention, parameterization", SqlExample = "EXEC sp_executesql N'SELECT * FROM Employees WHERE DeptId = @Id', N'@Id INT', @Id = 1", Tags = new() { "Dynamic SQL" } },

            // Transactions & Concurrency
            new() { Title = "Transactions", Category = "Transactions", Difficulty = "Medium", KeyConcepts = "BEGIN TRAN, COMMIT, ROLLBACK, SAVE TRAN, implicit vs explicit", SqlExample = "BEGIN TRAN; UPDATE...; IF @@ERROR <> 0 ROLLBACK ELSE COMMIT", Tags = new() { "Transactions", "ACID" } },
            new() { Title = "Isolation Levels", Category = "Transactions", Difficulty = "Hard", KeyConcepts = "READ UNCOMMITTED, READ COMMITTED, REPEATABLE READ, SERIALIZABLE, SNAPSHOT", SqlExample = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED", Tags = new() { "Isolation", "Concurrency" } },
            new() { Title = "Locking", Category = "Transactions", Difficulty = "Hard", KeyConcepts = "Shared, exclusive, update locks, lock escalation, NOLOCK, ROWLOCK hints", SqlExample = "SELECT * FROM Employees WITH (NOLOCK) WHERE DeptId = 1", Tags = new() { "Locking", "Concurrency" } },
            new() { Title = "Error Handling", Category = "Transactions", Difficulty = "Medium", KeyConcepts = "TRY-CATCH, THROW, RAISERROR, ERROR_MESSAGE(), XACT_STATE()", SqlExample = "BEGIN TRY ... END TRY BEGIN CATCH SELECT ERROR_MESSAGE(); THROW; END CATCH", Tags = new() { "Error Handling" } },

            // Data Types & Constraints
            new() { Title = "Data Types", Category = "Schema", Difficulty = "Easy", KeyConcepts = "INT, VARCHAR, NVARCHAR, DATETIME2, DECIMAL, BIT, UNIQUEIDENTIFIER, JSON", Tags = new() { "Data Types" } },
            new() { Title = "Constraints", Category = "Schema", Difficulty = "Easy", KeyConcepts = "PRIMARY KEY, FOREIGN KEY, UNIQUE, CHECK, DEFAULT, NOT NULL", SqlExample = "ALTER TABLE Employees ADD CONSTRAINT FK_Dept FOREIGN KEY (DeptId) REFERENCES Departments(Id)", Tags = new() { "Constraints", "Integrity" } },
            new() { Title = "Temporal Tables", Category = "Schema", Difficulty = "Medium", KeyConcepts = "System-versioned tables, history table, FOR SYSTEM_TIME, AS OF queries", SqlExample = "SELECT * FROM Employees FOR SYSTEM_TIME AS OF '2024-01-01'", Tags = new() { "Temporal", "History" } },
            new() { Title = "Computed Columns", Category = "Schema", Difficulty = "Easy", KeyConcepts = "PERSISTED, virtual, deterministic functions, indexed computed columns", SqlExample = "ALTER TABLE Employees ADD FullName AS (FirstName + ' ' + LastName) PERSISTED", Tags = new() { "Schema" } },

            // Advanced Topics
            new() { Title = "Partitioning", Category = "Advanced", Difficulty = "Hard", KeyConcepts = "Partition function, partition scheme, partition elimination, sliding window", Tags = new() { "Partitioning", "Large Tables" } },
            new() { Title = "JSON Support", Category = "Advanced", Difficulty = "Medium", KeyConcepts = "JSON_VALUE, JSON_QUERY, OPENJSON, FOR JSON, JSON_MODIFY", SqlExample = "SELECT JSON_VALUE(Data, '$.name') FROM JsonTable", Tags = new() { "JSON" } },
            new() { Title = "Full-Text Search", Category = "Advanced", Difficulty = "Medium", KeyConcepts = "Full-text index, CONTAINS, FREETEXT, CONTAINSTABLE, word breakers", Tags = new() { "Search" } },
            new() { Title = "SQL Server Agent Jobs", Category = "Administration", Difficulty = "Medium", KeyConcepts = "Scheduled jobs, job steps, notifications, maintenance plans", Tags = new() { "Automation", "Admin" } },
            new() { Title = "Backup & Recovery", Category = "Administration", Difficulty = "Medium", KeyConcepts = "Full, differential, log backups, recovery models, point-in-time restore", Tags = new() { "Backup", "DR" } },
            new() { Title = "Always On Availability Groups", Category = "Administration", Difficulty = "Hard", KeyConcepts = "HA/DR, readable secondaries, failover modes, listener", Tags = new() { "HA", "DR" } },
        };
    }

    public static List<DesignPatternTopic> GetDesignPatternTopics()
    {
        return new List<DesignPatternTopic>
        {
            // Creational Patterns
            new() { Title = "Singleton", Category = "Creational", Difficulty = "Easy", KeyConcepts = "Single instance, global access point, lazy initialization, thread safety", UseCases = "Logging, configuration, connection pools, caches", CodeExample = "public sealed class Singleton { private static readonly Lazy<Singleton> instance = new(() => new Singleton()); public static Singleton Instance => instance.Value; }", Tags = new() { "Creational", "Instance Control" } },
            new() { Title = "Factory Method", Category = "Creational", Difficulty = "Medium", KeyConcepts = "Virtual constructor, defers instantiation to subclasses, product interface", UseCases = "When class can't anticipate object types, frameworks, plugins", CodeExample = "abstract class Creator { abstract Product FactoryMethod(); } class ConcreteCreator : Creator { override Product FactoryMethod() => new ConcreteProduct(); }", Tags = new() { "Creational", "Polymorphism" } },
            new() { Title = "Abstract Factory", Category = "Creational", Difficulty = "Hard", KeyConcepts = "Factory of factories, product families, platform independence", UseCases = "Cross-platform UI, database providers, theming systems", CodeExample = "interface IUIFactory { IButton CreateButton(); ITextBox CreateTextBox(); } class WindowsFactory : IUIFactory { ... }", Tags = new() { "Creational", "Product Families" } },
            new() { Title = "Builder", Category = "Creational", Difficulty = "Medium", KeyConcepts = "Step-by-step construction, fluent interface, director class, immutable objects", UseCases = "Complex object construction, SQL query builders, HTTP requests", CodeExample = "var user = new UserBuilder().WithName('John').WithEmail('j@e.com').WithAge(30).Build();", Tags = new() { "Creational", "Fluent" } },
            new() { Title = "Prototype", Category = "Creational", Difficulty = "Medium", KeyConcepts = "Clone existing objects, shallow vs deep copy, ICloneable interface", UseCases = "Expensive object creation, undo mechanisms, configuration templates", CodeExample = "class Prototype : ICloneable { public object Clone() => MemberwiseClone(); }", Tags = new() { "Creational", "Cloning" } },

            // Structural Patterns
            new() { Title = "Adapter", Category = "Structural", Difficulty = "Easy", KeyConcepts = "Converts interface, wrapper, class vs object adapter, legacy integration", UseCases = "Third-party library integration, legacy code, incompatible interfaces", CodeExample = "class Adapter : ITarget { private Adaptee adaptee; public void Request() => adaptee.SpecificRequest(); }", Tags = new() { "Structural", "Wrapper" } },
            new() { Title = "Bridge", Category = "Structural", Difficulty = "Hard", KeyConcepts = "Separates abstraction from implementation, composition over inheritance", UseCases = "Platform-independent code, driver implementations, rendering engines", CodeExample = "abstract class Shape { protected IRenderer renderer; } class Circle : Shape { void Draw() => renderer.RenderCircle(); }", Tags = new() { "Structural", "Decoupling" } },
            new() { Title = "Composite", Category = "Structural", Difficulty = "Medium", KeyConcepts = "Tree structure, part-whole hierarchy, uniform treatment of objects", UseCases = "File systems, UI components, organization hierarchies, menus", CodeExample = "interface IComponent { void Operation(); } class Composite : IComponent { List<IComponent> children; void Operation() => children.ForEach(c => c.Operation()); }", Tags = new() { "Structural", "Tree" } },
            new() { Title = "Decorator", Category = "Structural", Difficulty = "Medium", KeyConcepts = "Dynamic behavior addition, wraps objects, alternative to subclassing", UseCases = "Stream wrappers, middleware, logging, caching, validation", CodeExample = "class LoggingDecorator : IService { private IService inner; void Execute() { Log(); inner.Execute(); } }", Tags = new() { "Structural", "Wrapper" } },
            new() { Title = "Facade", Category = "Structural", Difficulty = "Easy", KeyConcepts = "Simplified interface to complex subsystem, single entry point", UseCases = "Library wrappers, API simplification, subsystem decoupling", CodeExample = "class OrderFacade { void PlaceOrder() { inventory.Check(); payment.Process(); shipping.Ship(); } }", Tags = new() { "Structural", "Simplification" } },
            new() { Title = "Flyweight", Category = "Structural", Difficulty = "Hard", KeyConcepts = "Shared state, intrinsic vs extrinsic state, memory optimization", UseCases = "Text editors (characters), game objects, caching", CodeExample = "class CharacterFactory { Dictionary<char, Character> pool; Character GetChar(char c) { ... } }", Tags = new() { "Structural", "Memory" } },
            new() { Title = "Proxy", Category = "Structural", Difficulty = "Medium", KeyConcepts = "Placeholder/surrogate, access control, lazy loading, caching", UseCases = "Virtual proxy (lazy load), protection proxy (access), remote proxy, caching proxy", CodeExample = "class ImageProxy : IImage { private RealImage realImage; void Display() { if (realImage == null) realImage = new RealImage(); realImage.Display(); } }", Tags = new() { "Structural", "Proxy" } },

            // Behavioral Patterns
            new() { Title = "Chain of Responsibility", Category = "Behavioral", Difficulty = "Medium", KeyConcepts = "Request handling chain, decouples sender/receiver, dynamic chain", UseCases = "Middleware pipelines, event handling, approval workflows, logging", CodeExample = "abstract class Handler { protected Handler next; void HandleRequest(Request r) { if (CanHandle(r)) Process(r); else next?.HandleRequest(r); } }", Tags = new() { "Behavioral", "Chain" } },
            new() { Title = "Command", Category = "Behavioral", Difficulty = "Medium", KeyConcepts = "Encapsulates request as object, undo/redo, queuing, logging", UseCases = "Undo/redo, macro recording, task scheduling, transaction scripts", CodeExample = "interface ICommand { void Execute(); void Undo(); } class Invoker { void ExecuteCommand(ICommand cmd) { cmd.Execute(); history.Push(cmd); } }", Tags = new() { "Behavioral", "Encapsulation" } },
            new() { Title = "Iterator", Category = "Behavioral", Difficulty = "Easy", KeyConcepts = "Sequential access without exposing structure, IEnumerable, yield", UseCases = "Collection traversal, custom collections, lazy evaluation", CodeExample = "class TreeIterator : IEnumerator<Node> { public bool MoveNext() { ... } public Node Current { get; } }", Tags = new() { "Behavioral", "Traversal" } },
            new() { Title = "Mediator", Category = "Behavioral", Difficulty = "Medium", KeyConcepts = "Centralizes communication, reduces coupling, event aggregator", UseCases = "Chat rooms, air traffic control, UI component coordination, CQRS", CodeExample = "class ChatRoom : IMediator { void SendMessage(User from, string msg) { users.ForEach(u => u.Receive(from, msg)); } }", Tags = new() { "Behavioral", "Decoupling" } },
            new() { Title = "Memento", Category = "Behavioral", Difficulty = "Medium", KeyConcepts = "Captures object state, restore previous state, encapsulated state", UseCases = "Undo functionality, checkpoints, state snapshots, serialization", CodeExample = "class Originator { Memento Save() => new Memento(state); void Restore(Memento m) { state = m.State; } }", Tags = new() { "Behavioral", "State" } },
            new() { Title = "Observer", Category = "Behavioral", Difficulty = "Medium", KeyConcepts = "Publish-subscribe, event notification, loose coupling", UseCases = "Event systems, UI updates, notifications, reactive programming", CodeExample = "interface IObserver { void Update(); } class Subject { List<IObserver> observers; void Notify() => observers.ForEach(o => o.Update()); }", Tags = new() { "Behavioral", "Events" } },
            new() { Title = "State", Category = "Behavioral", Difficulty = "Medium", KeyConcepts = "Object behavior changes with state, state transitions, encapsulated states", UseCases = "Workflow engines, game states, order status, connection states", CodeExample = "class Context { IState state; void Request() => state.Handle(this); void SetState(IState s) => state = s; }", Tags = new() { "Behavioral", "State Machine" } },
            new() { Title = "Strategy", Category = "Behavioral", Difficulty = "Easy", KeyConcepts = "Interchangeable algorithms, runtime algorithm selection, composition", UseCases = "Sorting algorithms, payment methods, compression, validation", CodeExample = "class Context { IStrategy strategy; void Execute() => strategy.Algorithm(); } context.strategy = new ConcreteStrategyA();", Tags = new() { "Behavioral", "Algorithms" } },
            new() { Title = "Template Method", Category = "Behavioral", Difficulty = "Easy", KeyConcepts = "Algorithm skeleton in base class, subclasses override steps, hook methods", UseCases = "Frameworks, test fixtures, data parsing, report generation", CodeExample = "abstract class DataProcessor { void Process() { ReadData(); ProcessData(); WriteData(); } abstract void ProcessData(); }", Tags = new() { "Behavioral", "Inheritance" } },
            new() { Title = "Visitor", Category = "Behavioral", Difficulty = "Hard", KeyConcepts = "Separate algorithms from object structure, double dispatch", UseCases = "Compilers (AST), document export, tax calculation, serialization", CodeExample = "interface IVisitor { void Visit(ElementA a); void Visit(ElementB b); } class Element { void Accept(IVisitor v) => v.Visit(this); }", Tags = new() { "Behavioral", "Double Dispatch" } },

            // Other Patterns
            new() { Title = "Repository Pattern", Category = "Architectural", Difficulty = "Medium", KeyConcepts = "Abstracts data access, collection-like interface, decouples business logic from data", UseCases = "Data access abstraction, unit testing, multiple data sources", CodeExample = "interface IRepository<T> { T GetById(int id); void Add(T entity); void Update(T entity); }", Tags = new() { "Data Access", "Abstraction" } },
            new() { Title = "Unit of Work", Category = "Architectural", Difficulty = "Medium", KeyConcepts = "Tracks changes, single transaction, coordinates repositories", UseCases = "Transaction management, batching changes, EF Core DbContext", CodeExample = "interface IUnitOfWork { IRepository<User> Users { get; } Task CommitAsync(); }", Tags = new() { "Data Access", "Transaction" } },
            new() { Title = "Specification Pattern", Category = "Architectural", Difficulty = "Hard", KeyConcepts = "Business rules as objects, composable (And, Or, Not), reusable queries", UseCases = "Complex business rules, search filters, validation, query building", CodeExample = "class ActiveUserSpec : Specification<User> { override bool IsSatisfiedBy(User u) => u.IsActive; }", Tags = new() { "Business Rules", "Composition" } },
            new() { Title = "Null Object Pattern", Category = "Architectural", Difficulty = "Easy", KeyConcepts = "Provides default behavior, avoids null checks, polymorphic null", UseCases = "Default implementations, avoiding null reference exceptions", CodeExample = "class NullLogger : ILogger { void Log(string msg) { /* do nothing */ } }", Tags = new() { "Null Handling" } },
            new() { Title = "Service Locator", Category = "Architectural", Difficulty = "Medium", KeyConcepts = "Central registry for services, alternative to DI, anti-pattern concerns", UseCases = "Legacy systems, framework code (use DI instead when possible)", CodeExample = "class ServiceLocator { static T GetService<T>() => container.Resolve<T>(); }", Tags = new() { "Service Location" } },
            new() { Title = "Dependency Injection", Category = "Architectural", Difficulty = "Medium", KeyConcepts = "Inversion of control, constructor/property/method injection, containers", UseCases = "Loosely coupled systems, testing, configuration", CodeExample = "class OrderService { private readonly IRepository repo; public OrderService(IRepository repo) { this.repo = repo; } }", Tags = new() { "IoC", "DI" } },
        };
    }
}
