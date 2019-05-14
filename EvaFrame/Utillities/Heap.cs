using System;

namespace EvaFrame.Utilities
{
    /// <summary>
    /// Cấu trúc dữ liệu Min Heap.
    /// </summary>
    /// <typeparam name="T">
    /// Kiểu dữ liệu của các phần tử trong Heap. 
    /// Cần cài đặt interface <c>IComparable</c> và <c>ICloneable</c>.
    /// </typeparam>
    public class MinHeap<T> where T : IComparable, ICloneable
    {
        private class Node
        {
            public T value;
            public Node nextSibling, leftmostChild;

            public Node(T value)
            {
                this.value = (T)(value.Clone());
                this.nextSibling = this.leftmostChild = null;
            }
        }

        private Node root;
        private int count;
        /// <value>
        /// Số lượng phần tử trong Heap hiện tại.
        /// </value>
        public int Count { get { return count; } }

        /// <summary>
        /// Khởi tạo Heap rỗng.
        /// </summary>
        public MinHeap()
        {
            this.root = null;
            this.count = 0;
        }

        /// <summary>
        /// Trả lại giá trị ở đầu Heap (giá trị nhỏ nhất).
        /// </summary>
        /// <returns>Giá trị ở đầu Heap.</returns>
        public T Top()
        {
            if (root == null)
                return default(T);
            return root.value;
        }

        /// <summary>
        /// Thêm một phần tử vào Heap. Một object mới có giá trị bằng <c>value</c> sẽ được tạo ra, và đặt vào trong Heap.
        /// </summary>
        /// <param name="value">Giá trị bỏ vào Heap.</param>
        public void Push(T value)
        {
            root = merge(root, new Node(value));
            count++;
        }

        /// <summary>
        /// Loại bỏ phần tử ở đầu Heap (nhỏ nhất) ra khỏi Heap.
        /// </summary>
        public void Pop()
        {
            if (root == null)
                return;
            count--;
            root = mergeTwoPasses(root.leftmostChild);
        }

        private Node merge(Node heap1, Node heap2)
        {
            if (heap1 == null)
                return heap2;
            if (heap2 == null)
                return heap1;
            if (heap1.value.CompareTo(heap2.value) <= 0)
            {
                if (heap1.leftmostChild == null)
                    heap1.leftmostChild = heap2;
                else
                {
                    heap2.nextSibling = heap1.leftmostChild;
                    heap1.leftmostChild = heap2;
                }
                return heap1;
            }
            else
            {
                if (heap2.leftmostChild == null)
                    heap2.leftmostChild = heap1;
                else
                {
                    heap1.nextSibling = heap2.leftmostChild;
                    heap2.leftmostChild = heap1;
                }
                return heap2;
            }
        }

        private Node mergeTwoPasses(Node subheaps)
        {
            if (subheaps == null)
                return null;
            Node nextSibling = subheaps.nextSibling;
            if (nextSibling == null)
                return subheaps;
            Node otherSubheaps = nextSibling.nextSibling;
            subheaps.nextSibling = nextSibling.nextSibling = null;
            return merge(merge(subheaps, nextSibling), mergeTwoPasses(otherSubheaps));
        }
    }
}