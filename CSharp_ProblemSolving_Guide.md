# Comprehensive C# Problem-Solving Preparation Guide
## From Primary to Advanced Level

---

## Table of Contents
1. [Environment Setup](#1-environment-setup)
2. [C# Fundamentals](#2-c-fundamentals)
3. [Essential Data Structures](#3-essential-data-structures)
4. [Core Algorithms](#4-core-algorithms)
5. [Problem-Solving Techniques](#5-problem-solving-techniques)
6. [Advanced Topics](#6-advanced-topics)
7. [Common Patterns & Templates](#7-common-patterns--templates)
8. [Practice Strategy](#8-practice-strategy)
9. [Resources](#9-resources)

---

## 1. Environment Setup

### IDE Options
- **Visual Studio** (Full IDE)
- **Visual Studio Code** (Lightweight)
- **JetBrains Rider** (Professional)
- **Online**: LeetCode, HackerRank built-in editors

### Quick Template for Competitive Programming
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Your code here
    }
}
```

---

## 2. C# Fundamentals

### 2.1 Input/Output
```csharp
// Single input
int n = int.Parse(Console.ReadLine());

// Multiple inputs on one line
var inputs = Console.ReadLine().Split(' ');
int a = int.Parse(inputs[0]);
int b = int.Parse(inputs[1]);

// Using LINQ
var nums = Console.ReadLine().Split().Select(int.Parse).ToArray();

// Output
Console.WriteLine(result);
Console.Write(value); // No newline
```

### 2.2 String Manipulation
```csharp
string s = "hello";
char[] chars = s.ToCharArray();
string reversed = new string(s.Reverse().ToArray());
string substring = s.Substring(0, 3); // "hel"
bool contains = s.Contains("ell");
int index = s.IndexOf('e');
string[] parts = s.Split(',');
string joined = string.Join(",", parts);

// StringBuilder for efficiency
var sb = new StringBuilder();
sb.Append("text");
sb.Insert(0, "prefix");
string result = sb.ToString();
```

### 2.3 Arrays and Lists
```csharp
// Arrays
int[] arr = new int[5];
int[] arr2 = {1, 2, 3, 4, 5};
Array.Sort(arr);
Array.Reverse(arr);

// Lists (Dynamic arrays)
List<int> list = new List<int>();
list.Add(1);
list.Remove(1);
list.Sort();
list.Reverse();
int count = list.Count;
```

### 2.4 LINQ Essentials
```csharp
var numbers = new[] {1, 2, 3, 4, 5};

// Filtering
var evens = numbers.Where(x => x % 2 == 0);

// Transformation
var squares = numbers.Select(x => x * x);

// Aggregation
int sum = numbers.Sum();
int max = numbers.Max();
int min = numbers.Min();
double avg = numbers.Average();

// Ordering
var sorted = numbers.OrderBy(x => x);
var descending = numbers.OrderByDescending(x => x);

// Existence checks
bool any = numbers.Any(x => x > 3);
bool all = numbers.All(x => x > 0);

// Take/Skip
var first3 = numbers.Take(3);
var skip2 = numbers.Skip(2);
```

---

## 3. Essential Data Structures

### 3.1 Dictionary (Hash Map)
```csharp
Dictionary<string, int> dict = new Dictionary<string, int>();

// Add/Update
dict["key"] = 10;
dict.Add("key2", 20);

// Check and retrieve
if (dict.ContainsKey("key"))
{
    int value = dict["key"];
}

// TryGetValue (safer)
if (dict.TryGetValue("key", out int val))
{
    Console.WriteLine(val);
}

// Iterate
foreach (var kvp in dict)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

// GetValueOrDefault (C# 9+)
int value = dict.GetValueOrDefault("key", 0);
```

### 3.2 HashSet
```csharp
HashSet<int> set = new HashSet<int>();

set.Add(1);
set.Remove(1);
bool exists = set.Contains(1);

// Set operations
var set2 = new HashSet<int> {3, 4, 5};
set.UnionWith(set2);        // Union
set.IntersectWith(set2);    // Intersection
set.ExceptWith(set2);       // Difference
```

### 3.3 Queue (FIFO)
```csharp
Queue<int> queue = new Queue<int>();

queue.Enqueue(1);
queue.Enqueue(2);
int first = queue.Dequeue();
int peek = queue.Peek(); // Look without removing
int count = queue.Count;
```

### 3.4 Stack (LIFO)
```csharp
Stack<int> stack = new Stack<int>();

stack.Push(1);
stack.Push(2);
int top = stack.Pop();
int peek = stack.Peek();
bool empty = stack.Count == 0;
```

### 3.5 Priority Queue (Heap)
```csharp
// .NET 6+
PriorityQueue<string, int> pq = new PriorityQueue<string, int>();

pq.Enqueue("item1", 5);  // item, priority
pq.Enqueue("item2", 1);
string highest = pq.Dequeue(); // Gets lowest priority first

// For max heap, negate priorities
pq.Enqueue("item", -priority);

// Custom implementation for older .NET
class MinHeap<T> where T : IComparable<T>
{
    private List<T> heap = new List<T>();

    public void Insert(T item)
    {
        heap.Add(item);
        HeapifyUp(heap.Count - 1);
    }

    public T ExtractMin()
    {
        if (heap.Count == 0) throw new InvalidOperationException();
        T min = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);
        return min;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (heap[index].CompareTo(heap[parent]) >= 0) break;
            Swap(index, parent);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int smallest = index;

            if (left < heap.Count && heap[left].CompareTo(heap[smallest]) < 0)
                smallest = left;
            if (right < heap.Count && heap[right].CompareTo(heap[smallest]) < 0)
                smallest = right;

            if (smallest == index) break;
            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        T temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }

    public int Count => heap.Count;
}
```

### 3.6 Linked List
```csharp
// Built-in
LinkedList<int> ll = new LinkedList<int>();
ll.AddFirst(1);
ll.AddLast(2);
ll.Remove(1);

// Custom implementation
class ListNode
{
    public int val;
    public ListNode next;
    public ListNode(int x) { val = x; }
}
```

### 3.7 Trees
```csharp
// Binary Tree Node
class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int x) { val = x; }
}

// Binary Search Tree Operations
class BST
{
    public TreeNode Insert(TreeNode root, int val)
    {
        if (root == null) return new TreeNode(val);
        if (val < root.val)
            root.left = Insert(root.left, val);
        else
            root.right = Insert(root.right, val);
        return root;
    }

    public bool Search(TreeNode root, int val)
    {
        if (root == null) return false;
        if (root.val == val) return true;
        return val < root.val ? Search(root.left, val) : Search(root.right, val);
    }
}
```

### 3.8 Graph
```csharp
// Adjacency List (most common)
Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();

void AddEdge(int u, int v)
{
    if (!graph.ContainsKey(u)) graph[u] = new List<int>();
    if (!graph.ContainsKey(v)) graph[v] = new List<int>();
    graph[u].Add(v);
    // For undirected: graph[v].Add(u);
}

// Adjacency Matrix
int[,] matrix = new int[n, n];
matrix[u, v] = 1; // Edge from u to v
```

---

## 4. Core Algorithms

### 4.1 Sorting

#### Quick Sort
```csharp
void QuickSort(int[] arr, int low, int high)
{
    if (low < high)
    {
        int pi = Partition(arr, low, high);
        QuickSort(arr, low, pi - 1);
        QuickSort(arr, pi + 1, high);
    }
}

int Partition(int[] arr, int low, int high)
{
    int pivot = arr[high];
    int i = low - 1;

    for (int j = low; j < high; j++)
    {
        if (arr[j] < pivot)
        {
            i++;
            Swap(arr, i, j);
        }
    }
    Swap(arr, i + 1, high);
    return i + 1;
}

void Swap(int[] arr, int i, int j)
{
    int temp = arr[i];
    arr[i] = arr[j];
    arr[j] = temp;
}
```

#### Merge Sort
```csharp
void MergeSort(int[] arr, int left, int right)
{
    if (left < right)
    {
        int mid = left + (right - left) / 2;
        MergeSort(arr, left, mid);
        MergeSort(arr, mid + 1, right);
        Merge(arr, left, mid, right);
    }
}

void Merge(int[] arr, int left, int mid, int right)
{
    int n1 = mid - left + 1;
    int n2 = right - mid;

    int[] L = new int[n1];
    int[] R = new int[n2];

    Array.Copy(arr, left, L, 0, n1);
    Array.Copy(arr, mid + 1, R, 0, n2);

    int i = 0, j = 0, k = left;

    while (i < n1 && j < n2)
    {
        if (L[i] <= R[j])
            arr[k++] = L[i++];
        else
            arr[k++] = R[j++];
    }

    while (i < n1) arr[k++] = L[i++];
    while (j < n2) arr[k++] = R[j++];
}
```

### 4.2 Searching

#### Binary Search
```csharp
int BinarySearch(int[] arr, int target)
{
    int left = 0, right = arr.Length - 1;

    while (left <= right)
    {
        int mid = left + (right - left) / 2;

        if (arr[mid] == target)
            return mid;
        else if (arr[mid] < target)
            left = mid + 1;
        else
            right = mid - 1;
    }

    return -1;
}

// Find first occurrence
int FindFirst(int[] arr, int target)
{
    int left = 0, right = arr.Length - 1;
    int result = -1;

    while (left <= right)
    {
        int mid = left + (right - left) / 2;

        if (arr[mid] == target)
        {
            result = mid;
            right = mid - 1; // Continue searching left
        }
        else if (arr[mid] < target)
            left = mid + 1;
        else
            right = mid - 1;
    }

    return result;
}
```

### 4.3 Graph Traversal

#### BFS (Breadth-First Search)
```csharp
void BFS(Dictionary<int, List<int>> graph, int start)
{
    HashSet<int> visited = new HashSet<int>();
    Queue<int> queue = new Queue<int>();

    queue.Enqueue(start);
    visited.Add(start);

    while (queue.Count > 0)
    {
        int node = queue.Dequeue();
        Console.WriteLine(node);

        if (graph.ContainsKey(node))
        {
            foreach (int neighbor in graph[node])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }
    }
}
```

#### DFS (Depth-First Search)
```csharp
void DFS(Dictionary<int, List<int>> graph, int node, HashSet<int> visited)
{
    visited.Add(node);
    Console.WriteLine(node);

    if (graph.ContainsKey(node))
    {
        foreach (int neighbor in graph[node])
        {
            if (!visited.Contains(neighbor))
            {
                DFS(graph, neighbor, visited);
            }
        }
    }
}

// Iterative DFS
void DFSIterative(Dictionary<int, List<int>> graph, int start)
{
    HashSet<int> visited = new HashSet<int>();
    Stack<int> stack = new Stack<int>();

    stack.Push(start);

    while (stack.Count > 0)
    {
        int node = stack.Pop();

        if (!visited.Contains(node))
        {
            visited.Add(node);
            Console.WriteLine(node);

            if (graph.ContainsKey(node))
            {
                foreach (int neighbor in graph[node])
                {
                    if (!visited.Contains(neighbor))
                        stack.Push(neighbor);
                }
            }
        }
    }
}
```

### 4.4 Dijkstra's Algorithm (Shortest Path)
```csharp
Dictionary<int, int> Dijkstra(Dictionary<int, List<(int node, int weight)>> graph, int start)
{
    var distances = new Dictionary<int, int>();
    var pq = new PriorityQueue<int, int>();

    foreach (var node in graph.Keys)
        distances[node] = int.MaxValue;

    distances[start] = 0;
    pq.Enqueue(start, 0);

    while (pq.Count > 0)
    {
        int current = pq.Dequeue();

        if (graph.ContainsKey(current))
        {
            foreach (var (neighbor, weight) in graph[current])
            {
                int newDist = distances[current] + weight;

                if (newDist < distances[neighbor])
                {
                    distances[neighbor] = newDist;
                    pq.Enqueue(neighbor, newDist);
                }
            }
        }
    }

    return distances;
}
```

---

## 5. Problem-Solving Techniques

### 5.1 Two Pointers
```csharp
// Example: Find pair with sum
bool HasPairWithSum(int[] arr, int target)
{
    Array.Sort(arr);
    int left = 0, right = arr.Length - 1;

    while (left < right)
    {
        int sum = arr[left] + arr[right];
        if (sum == target)
            return true;
        else if (sum < target)
            left++;
        else
            right--;
    }

    return false;
}

// Example: Remove duplicates from sorted array
int RemoveDuplicates(int[] nums)
{
    if (nums.Length == 0) return 0;

    int i = 0;
    for (int j = 1; j < nums.Length; j++)
    {
        if (nums[j] != nums[i])
        {
            i++;
            nums[i] = nums[j];
        }
    }

    return i + 1;
}
```

### 5.2 Sliding Window
```csharp
// Example: Maximum sum subarray of size k
int MaxSumSubarray(int[] arr, int k)
{
    int maxSum = 0, windowSum = 0;

    // First window
    for (int i = 0; i < k; i++)
        windowSum += arr[i];

    maxSum = windowSum;

    // Slide the window
    for (int i = k; i < arr.Length; i++)
    {
        windowSum += arr[i] - arr[i - k];
        maxSum = Math.Max(maxSum, windowSum);
    }

    return maxSum;
}

// Variable size window
int LongestSubstringKDistinct(string s, int k)
{
    var charCount = new Dictionary<char, int>();
    int left = 0, maxLen = 0;

    for (int right = 0; right < s.Length; right++)
    {
        charCount[s[right]] = charCount.GetValueOrDefault(s[right], 0) + 1;

        while (charCount.Count > k)
        {
            charCount[s[left]]--;
            if (charCount[s[left]] == 0)
                charCount.Remove(s[left]);
            left++;
        }

        maxLen = Math.Max(maxLen, right - left + 1);
    }

    return maxLen;
}
```

### 5.3 Dynamic Programming

#### Top-Down (Memoization)
```csharp
// Fibonacci
int Fib(int n, Dictionary<int, int> memo = null)
{
    memo ??= new Dictionary<int, int>();

    if (n <= 1) return n;
    if (memo.ContainsKey(n)) return memo[n];

    memo[n] = Fib(n - 1, memo) + Fib(n - 2, memo);
    return memo[n];
}
```

#### Bottom-Up (Tabulation)
```csharp
// Fibonacci
int FibBottomUp(int n)
{
    if (n <= 1) return n;

    int[] dp = new int[n + 1];
    dp[0] = 0;
    dp[1] = 1;

    for (int i = 2; i <= n; i++)
    {
        dp[i] = dp[i - 1] + dp[i - 2];
    }

    return dp[n];
}

// Coin Change
int CoinChange(int[] coins, int amount)
{
    int[] dp = new int[amount + 1];
    Array.Fill(dp, amount + 1);
    dp[0] = 0;

    for (int i = 1; i <= amount; i++)
    {
        foreach (int coin in coins)
        {
            if (i >= coin)
            {
                dp[i] = Math.Min(dp[i], dp[i - coin] + 1);
            }
        }
    }

    return dp[amount] > amount ? -1 : dp[amount];
}

// Longest Common Subsequence
int LCS(string text1, string text2)
{
    int m = text1.Length, n = text2.Length;
    int[,] dp = new int[m + 1, n + 1];

    for (int i = 1; i <= m; i++)
    {
        for (int j = 1; j <= n; j++)
        {
            if (text1[i - 1] == text2[j - 1])
                dp[i, j] = dp[i - 1, j - 1] + 1;
            else
                dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
        }
    }

    return dp[m, n];
}

// 0/1 Knapsack
int Knapsack(int[] weights, int[] values, int capacity)
{
    int n = weights.Length;
    int[,] dp = new int[n + 1, capacity + 1];

    for (int i = 1; i <= n; i++)
    {
        for (int w = 1; w <= capacity; w++)
        {
            if (weights[i - 1] <= w)
            {
                dp[i, w] = Math.Max(
                    values[i - 1] + dp[i - 1, w - weights[i - 1]],
                    dp[i - 1, w]
                );
            }
            else
            {
                dp[i, w] = dp[i - 1, w];
            }
        }
    }

    return dp[n, capacity];
}
```

### 5.4 Backtracking
```csharp
// Permutations
List<List<int>> Permutations(int[] nums)
{
    var result = new List<List<int>>();
    Backtrack(nums, new List<int>(), result);
    return result;
}

void Backtrack(int[] nums, List<int> current, List<List<int>> result)
{
    if (current.Count == nums.Length)
    {
        result.Add(new List<int>(current));
        return;
    }

    foreach (int num in nums)
    {
        if (current.Contains(num)) continue;

        current.Add(num);
        Backtrack(nums, current, result);
        current.RemoveAt(current.Count - 1);
    }
}

// Subsets
List<List<int>> Subsets(int[] nums)
{
    var result = new List<List<int>>();
    BacktrackSubsets(nums, 0, new List<int>(), result);
    return result;
}

void BacktrackSubsets(int[] nums, int start, List<int> current, List<List<int>> result)
{
    result.Add(new List<int>(current));

    for (int i = start; i < nums.Length; i++)
    {
        current.Add(nums[i]);
        BacktrackSubsets(nums, i + 1, current, result);
        current.RemoveAt(current.Count - 1);
    }
}

// N-Queens
List<List<string>> SolveNQueens(int n)
{
    var result = new List<List<string>>();
    var board = new char[n][];
    for (int i = 0; i < n; i++)
    {
        board[i] = new char[n];
        Array.Fill(board[i], '.');
    }

    BacktrackQueens(board, 0, result);
    return result;
}

void BacktrackQueens(char[][] board, int row, List<List<string>> result)
{
    if (row == board.Length)
    {
        result.Add(board.Select(r => new string(r)).ToList());
        return;
    }

    for (int col = 0; col < board.Length; col++)
    {
        if (IsValid(board, row, col))
        {
            board[row][col] = 'Q';
            BacktrackQueens(board, row + 1, result);
            board[row][col] = '.';
        }
    }
}

bool IsValid(char[][] board, int row, int col)
{
    // Check column
    for (int i = 0; i < row; i++)
        if (board[i][col] == 'Q') return false;

    // Check diagonal
    for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
        if (board[i][j] == 'Q') return false;

    // Check anti-diagonal
    for (int i = row - 1, j = col + 1; i >= 0 && j < board.Length; i--, j++)
        if (board[i][j] == 'Q') return false;

    return true;
}
```

### 5.5 Greedy Algorithms
```csharp
// Activity Selection
int MaxActivities(int[] start, int[] end)
{
    var activities = start.Select((s, i) => (start: s, end: end[i]))
                         .OrderBy(a => a.end)
                         .ToArray();

    int count = 1;
    int lastEnd = activities[0].end;

    for (int i = 1; i < activities.Length; i++)
    {
        if (activities[i].start >= lastEnd)
        {
            count++;
            lastEnd = activities[i].end;
        }
    }

    return count;
}

// Jump Game
bool CanJump(int[] nums)
{
    int maxReach = 0;

    for (int i = 0; i < nums.Length; i++)
    {
        if (i > maxReach) return false;
        maxReach = Math.Max(maxReach, i + nums[i]);
        if (maxReach >= nums.Length - 1) return true;
    }

    return true;
}
```

### 5.6 Bit Manipulation
```csharp
// Check if bit is set
bool IsBitSet(int num, int pos) => (num & (1 << pos)) != 0;

// Set bit
int SetBit(int num, int pos) => num | (1 << pos);

// Clear bit
int ClearBit(int num, int pos) => num & ~(1 << pos);

// Toggle bit
int ToggleBit(int num, int pos) => num ^ (1 << pos);

// Count set bits
int CountSetBits(int n)
{
    int count = 0;
    while (n > 0)
    {
        count += n & 1;
        n >>= 1;
    }
    return count;
}

// Check if power of 2
bool IsPowerOfTwo(int n) => n > 0 && (n & (n - 1)) == 0;

// XOR properties
// a ^ a = 0
// a ^ 0 = a
// Find single number in array where all others appear twice
int SingleNumber(int[] nums) => nums.Aggregate((a, b) => a ^ b);
```

---

## 6. Advanced Topics

### 6.1 Trie (Prefix Tree)
```csharp
class TrieNode
{
    public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();
    public bool IsEndOfWord = false;
}

class Trie
{
    private TrieNode root = new TrieNode();

    public void Insert(string word)
    {
        var node = root;
        foreach (char c in word)
        {
            if (!node.Children.ContainsKey(c))
                node.Children[c] = new TrieNode();
            node = node.Children[c];
        }
        node.IsEndOfWord = true;
    }

    public bool Search(string word)
    {
        var node = FindNode(word);
        return node != null && node.IsEndOfWord;
    }

    public bool StartsWith(string prefix)
    {
        return FindNode(prefix) != null;
    }

    private TrieNode FindNode(string prefix)
    {
        var node = root;
        foreach (char c in prefix)
        {
            if (!node.Children.ContainsKey(c))
                return null;
            node = node.Children[c];
        }
        return node;
    }
}
```

### 6.2 Union-Find (Disjoint Set)
```csharp
class UnionFind
{
    private int[] parent;
    private int[] rank;

    public UnionFind(int n)
    {
        parent = new int[n];
        rank = new int[n];
        for (int i = 0; i < n; i++)
            parent[i] = i;
    }

    public int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]); // Path compression
        return parent[x];
    }

    public bool Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);

        if (rootX == rootY)
            return false;

        // Union by rank
        if (rank[rootX] < rank[rootY])
            parent[rootX] = rootY;
        else if (rank[rootX] > rank[rootY])
            parent[rootY] = rootX;
        else
        {
            parent[rootY] = rootX;
            rank[rootX]++;
        }

        return true;
    }

    public bool Connected(int x, int y)
    {
        return Find(x) == Find(y);
    }
}
```

### 6.3 Segment Tree
```csharp
class SegmentTree
{
    private int[] tree;
    private int n;

    public SegmentTree(int[] arr)
    {
        n = arr.Length;
        tree = new int[4 * n];
        Build(arr, 0, 0, n - 1);
    }

    private void Build(int[] arr, int node, int start, int end)
    {
        if (start == end)
        {
            tree[node] = arr[start];
            return;
        }

        int mid = (start + end) / 2;
        Build(arr, 2 * node + 1, start, mid);
        Build(arr, 2 * node + 2, mid + 1, end);
        tree[node] = tree[2 * node + 1] + tree[2 * node + 2];
    }

    public void Update(int index, int value)
    {
        Update(0, 0, n - 1, index, value);
    }

    private void Update(int node, int start, int end, int index, int value)
    {
        if (start == end)
        {
            tree[node] = value;
            return;
        }

        int mid = (start + end) / 2;
        if (index <= mid)
            Update(2 * node + 1, start, mid, index, value);
        else
            Update(2 * node + 2, mid + 1, end, index, value);

        tree[node] = tree[2 * node + 1] + tree[2 * node + 2];
    }

    public int Query(int left, int right)
    {
        return Query(0, 0, n - 1, left, right);
    }

    private int Query(int node, int start, int end, int left, int right)
    {
        if (right < start || left > end)
            return 0;

        if (left <= start && end <= right)
            return tree[node];

        int mid = (start + end) / 2;
        return Query(2 * node + 1, start, mid, left, right) +
               Query(2 * node + 2, mid + 1, end, left, right);
    }
}
```

### 6.4 Topological Sort
```csharp
// Kahn's Algorithm (BFS)
List<int> TopologicalSort(int n, List<(int from, int to)> edges)
{
    var graph = new Dictionary<int, List<int>>();
    var inDegree = new int[n];

    for (int i = 0; i < n; i++)
        graph[i] = new List<int>();

    foreach (var (from, to) in edges)
    {
        graph[from].Add(to);
        inDegree[to]++;
    }

    var queue = new Queue<int>();
    for (int i = 0; i < n; i++)
    {
        if (inDegree[i] == 0)
            queue.Enqueue(i);
    }

    var result = new List<int>();

    while (queue.Count > 0)
    {
        int node = queue.Dequeue();
        result.Add(node);

        foreach (int neighbor in graph[node])
        {
            inDegree[neighbor]--;
            if (inDegree[neighbor] == 0)
                queue.Enqueue(neighbor);
        }
    }

    return result.Count == n ? result : new List<int>();
}

// DFS-based
List<int> TopologicalSortDFS(Dictionary<int, List<int>> graph)
{
    var visited = new HashSet<int>();
    var stack = new Stack<int>();

    foreach (var node in graph.Keys)
    {
        if (!visited.Contains(node))
            DFSTopSort(graph, node, visited, stack);
    }

    return stack.ToList();
}

void DFSTopSort(Dictionary<int, List<int>> graph, int node, HashSet<int> visited, Stack<int> stack)
{
    visited.Add(node);

    if (graph.ContainsKey(node))
    {
        foreach (int neighbor in graph[node])
        {
            if (!visited.Contains(neighbor))
                DFSTopSort(graph, neighbor, visited, stack);
        }
    }

    stack.Push(node);
}
```

### 6.5 LRU Cache
```csharp
class LRUCache
{
    private class Node
    {
        public int Key, Value;
        public Node Prev, Next;
    }

    private Dictionary<int, Node> cache;
    private Node head, tail;
    private int capacity;

    public LRUCache(int capacity)
    {
        this.capacity = capacity;
        cache = new Dictionary<int, Node>();
        head = new Node();
        tail = new Node();
        head.Next = tail;
        tail.Prev = head;
    }

    public int Get(int key)
    {
        if (!cache.ContainsKey(key))
            return -1;

        var node = cache[key];
        MoveToHead(node);
        return node.Value;
    }

    public void Put(int key, int value)
    {
        if (cache.ContainsKey(key))
        {
            var node = cache[key];
            node.Value = value;
            MoveToHead(node);
        }
        else
        {
            var node = new Node { Key = key, Value = value };
            cache[key] = node;
            AddToHead(node);

            if (cache.Count > capacity)
            {
                var removed = RemoveTail();
                cache.Remove(removed.Key);
            }
        }
    }

    private void AddToHead(Node node)
    {
        node.Next = head.Next;
        node.Prev = head;
        head.Next.Prev = node;
        head.Next = node;
    }

    private void RemoveNode(Node node)
    {
        node.Prev.Next = node.Next;
        node.Next.Prev = node.Prev;
    }

    private void MoveToHead(Node node)
    {
        RemoveNode(node);
        AddToHead(node);
    }

    private Node RemoveTail()
    {
        var node = tail.Prev;
        RemoveNode(node);
        return node;
    }
}
```

---

## 7. Common Patterns & Templates

### 7.1 Matrix Traversal
```csharp
// 4 directions
int[][] directions = new int[][] {
    new int[] {-1, 0}, // up
    new int[] {1, 0},  // down
    new int[] {0, -1}, // left
    new int[] {0, 1}   // right
};

// 8 directions (including diagonals)
int[][] directions8 = new int[][] {
    new int[] {-1, -1}, new int[] {-1, 0}, new int[] {-1, 1},
    new int[] {0, -1},                      new int[] {0, 1},
    new int[] {1, -1},  new int[] {1, 0},  new int[] {1, 1}
};

bool IsValid(int row, int col, int rows, int cols)
{
    return row >= 0 && row < rows && col >= 0 && col < cols;
}

// DFS on Matrix
void DFSMatrix(int[][] matrix, int row, int col, bool[][] visited)
{
    if (!IsValid(row, col, matrix.Length, matrix[0].Length) || visited[row][col])
        return;

    visited[row][col] = true;

    foreach (var dir in directions)
    {
        DFSMatrix(matrix, row + dir[0], col + dir[1], visited);
    }
}
```

### 7.2 Binary Tree Traversal
```csharp
// Inorder (Left, Root, Right)
void Inorder(TreeNode root, List<int> result)
{
    if (root == null) return;
    Inorder(root.left, result);
    result.Add(root.val);
    Inorder(root.right, result);
}

// Preorder (Root, Left, Right)
void Preorder(TreeNode root, List<int> result)
{
    if (root == null) return;
    result.Add(root.val);
    Preorder(root.left, result);
    Preorder(root.right, result);
}

// Postorder (Left, Right, Root)
void Postorder(TreeNode root, List<int> result)
{
    if (root == null) return;
    Postorder(root.left, result);
    Postorder(root.right, result);
    result.Add(root.val);
}

// Level Order (BFS)
List<List<int>> LevelOrder(TreeNode root)
{
    var result = new List<List<int>>();
    if (root == null) return result;

    var queue = new Queue<TreeNode>();
    queue.Enqueue(root);

    while (queue.Count > 0)
    {
        int levelSize = queue.Count;
        var level = new List<int>();

        for (int i = 0; i < levelSize; i++)
        {
            var node = queue.Dequeue();
            level.Add(node.val);

            if (node.left != null) queue.Enqueue(node.left);
            if (node.right != null) queue.Enqueue(node.right);
        }

        result.Add(level);
    }

    return result;
}
```

### 7.3 String Pattern Matching
```csharp
// KMP Algorithm
int[] BuildLPS(string pattern)
{
    int[] lps = new int[pattern.Length];
    int len = 0;
    int i = 1;

    while (i < pattern.Length)
    {
        if (pattern[i] == pattern[len])
        {
            len++;
            lps[i] = len;
            i++;
        }
        else
        {
            if (len != 0)
                len = lps[len - 1];
            else
            {
                lps[i] = 0;
                i++;
            }
        }
    }

    return lps;
}

List<int> KMPSearch(string text, string pattern)
{
    var result = new List<int>();
    int[] lps = BuildLPS(pattern);

    int i = 0, j = 0;
    while (i < text.Length)
    {
        if (text[i] == pattern[j])
        {
            i++;
            j++;
        }

        if (j == pattern.Length)
        {
            result.Add(i - j);
            j = lps[j - 1];
        }
        else if (i < text.Length && text[i] != pattern[j])
        {
            if (j != 0)
                j = lps[j - 1];
            else
                i++;
        }
    }

    return result;
}
```

### 7.4 Interval Problems
```csharp
// Merge Intervals
int[][] MergeIntervals(int[][] intervals)
{
    if (intervals.Length == 0) return intervals;

    Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));
    var result = new List<int[]>();
    result.Add(intervals[0]);

    for (int i = 1; i < intervals.Length; i++)
    {
        var last = result[result.Count - 1];

        if (intervals[i][0] <= last[1])
        {
            last[1] = Math.Max(last[1], intervals[i][1]);
        }
        else
        {
            result.Add(intervals[i]);
        }
    }

    return result.ToArray();
}

// Insert Interval
int[][] InsertInterval(int[][] intervals, int[] newInterval)
{
    var result = new List<int[]>();
    int i = 0;

    // Add all intervals before newInterval
    while (i < intervals.Length && intervals[i][1] < newInterval[0])
    {
        result.Add(intervals[i]);
        i++;
    }

    // Merge overlapping intervals
    while (i < intervals.Length && intervals[i][0] <= newInterval[1])
    {
        newInterval[0] = Math.Min(newInterval[0], intervals[i][0]);
        newInterval[1] = Math.Max(newInterval[1], intervals[i][1]);
        i++;
    }
    result.Add(newInterval);

    // Add remaining intervals
    while (i < intervals.Length)
    {
        result.Add(intervals[i]);
        i++;
    }

    return result.ToArray();
}
```

---

## 8. Practice Strategy

### 8.1 Learning Path
1. **Week 1-2: Foundations**
   - Arrays and Strings
   - Basic sorting and searching
   - Two pointers technique

2. **Week 3-4: Data Structures**
   - Stack, Queue, Deque
   - HashSet, HashMap
   - Linked Lists

3. **Week 5-6: Trees and Graphs**
   - Binary trees (traversal, BST)
   - BFS and DFS
   - Basic graph problems

4. **Week 7-8: Advanced Techniques**
   - Dynamic Programming (basic)
   - Backtracking
   - Sliding Window

5. **Week 9-10: Optimization**
   - Advanced DP
   - Greedy algorithms
   - Bit manipulation

6. **Week 11-12: Advanced Data Structures**
   - Heap/Priority Queue
   - Trie
   - Union-Find
   - Segment Tree

### 8.2 Problem-Solving Approach
1. **Understand the Problem**
   - Read carefully
   - Identify inputs/outputs
   - Clarify edge cases
   - Work through examples

2. **Plan**
   - Identify pattern
   - Choose data structure
   - Design algorithm
   - Analyze complexity

3. **Implement**
   - Write clean code
   - Handle edge cases
   - Add comments if complex

4. **Test**
   - Test with examples
   - Test edge cases
   - Test large inputs

5. **Optimize**
   - Review time complexity
   - Review space complexity
   - Refactor if needed

### 8.3 Daily Practice
- **Easy**: 30 minutes (warm-up)
- **Medium**: 45-60 minutes (main practice)
- **Hard**: 60+ minutes (weekends/optional)

### 8.4 Topic Distribution
- Arrays & Strings: 30%
- Trees & Graphs: 25%
- Dynamic Programming: 20%
- Others: 25%

---

## 9. Resources

### 9.1 Online Platforms
- **LeetCode** - Best for interview prep
- **HackerRank** - Good for beginners
- **CodeForces** - Competitive programming
- **AtCoder** - Japanese competitive programming
- **TopCoder** - Algorithm competitions

### 9.2 Practice by Pattern
| Pattern | LeetCode Problems |
|---------|-------------------|
| Two Pointers | 15, 11, 42, 125, 344 |
| Sliding Window | 3, 76, 438, 567, 239 |
| Binary Search | 33, 34, 35, 153, 278 |
| DFS/BFS | 200, 133, 207, 695, 994 |
| DP | 70, 198, 322, 300, 1143 |
| Backtracking | 46, 78, 39, 51, 79 |
| Greedy | 55, 45, 435, 452, 621 |

### 9.3 Books
- "Cracking the Coding Interview" - Gayle Laakmann McDowell
- "Introduction to Algorithms" - CLRS
- "Algorithm Design Manual" - Steven Skiena
- "Competitive Programming" - Steven & Felix Halim

### 9.4 Complexity Cheat Sheet

| Data Structure | Access | Search | Insert | Delete |
|----------------|--------|--------|--------|--------|
| Array | O(1) | O(n) | O(n) | O(n) |
| Linked List | O(n) | O(n) | O(1) | O(1) |
| Stack | O(n) | O(n) | O(1) | O(1) |
| Queue | O(n) | O(n) | O(1) | O(1) |
| Hash Table | - | O(1) | O(1) | O(1) |
| Binary Tree | O(n) | O(n) | O(n) | O(n) |
| BST | O(log n) | O(log n) | O(log n) | O(log n) |
| Heap | - | O(n) | O(log n) | O(log n) |

| Algorithm | Time | Space |
|-----------|------|-------|
| Quick Sort | O(n log n) | O(log n) |
| Merge Sort | O(n log n) | O(n) |
| Heap Sort | O(n log n) | O(1) |
| Binary Search | O(log n) | O(1) |
| BFS | O(V + E) | O(V) |
| DFS | O(V + E) | O(V) |
| Dijkstra | O((V + E) log V) | O(V) |

---

## Tips for Success

1. **Consistency** - Practice daily, even if just 30 minutes
2. **Understanding over Memorization** - Know why, not just how
3. **Write Code by Hand** - Helps in interviews
4. **Time Yourself** - Build speed gradually
5. **Review Solutions** - Learn from optimal approaches
6. **Discuss with Others** - Join communities
7. **Mock Interviews** - Practice under pressure
8. **Track Progress** - Maintain a problem log
9. **Focus on Patterns** - Not individual problems
10. **Stay Positive** - Progress takes time

---

**Good luck with your preparation!**
