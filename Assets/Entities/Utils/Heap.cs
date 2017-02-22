using System;

public class Heap<T> where T : IHeapItem<T>
{
	T[] items;
	int itemsCount;

	public Heap (int maxHeapSize)
	{
		items = new T[maxHeapSize];
	}

	public void Add (T item)
	{
		item.HeapIndex = itemsCount;
		items [itemsCount] = item;
		SortUp (item);
		itemsCount++;
	}

	public T RemoveFirst ()
	{
		T item = items [0];
		itemsCount--;
		items [0] = items [itemsCount];
		items [0].HeapIndex = 0;
		SortDown (items [0]);
		return item;
	}

	public void UpdateItem (T item)
	{
		SortUp (item);
	}

	public int Count {
		get {
			return itemsCount;
		}
	}

	public bool Contains (T item)
	{
		return Equals (items [item.HeapIndex], item);
	}

	void SortUp (T item)
	{
		int parentIndex = (item.HeapIndex - 1) / 2;
		while (true) {
			T parentItem = items [parentIndex];
			if (item.CompareTo (parentItem) > 0) {
				Swap (item, parentItem);
			} else {
				break;
			}
			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	void SortDown (T item)
	{
		while (true) {
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = childIndexLeft + 1;
			int swapIndex = 0;

			if (childIndexLeft < itemsCount) {
				swapIndex = childIndexLeft;
				if (childIndexRight < itemsCount) {
					if (items [childIndexLeft].CompareTo (items [childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo (items [swapIndex]) < 0) {
					Swap (item, items [swapIndex]);
				} else {
					break;
				}
			} else {
				break;
			}
		}	
	}

	void Swap (T itemA, T itemB)
	{
		items [itemA.HeapIndex] = itemB;
		items [itemB.HeapIndex] = itemA;
		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}

public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex {
		get;
		set;
	}
}