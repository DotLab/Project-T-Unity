﻿namespace Futilef {
	[Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
	public unsafe struct Pool2 {
		#if FDB
		public static int Type = Fdb.NewType("Pool2");
		public int type;
		#endif

		static int HeadSize, TailSize;
		static bool Initialized;

		public static void TypeInit() {
			HeadSize = 3 * sizeof(int);
			TailSize = sizeof(int);
			Initialized = true;
		}

		public int len;
		public byte *arr;
		public long shift;

		public static Pool2 *New() {
			var pool = (Pool2 *)Mem.Malloc(sizeof(Pool2));
			Init(pool);
			return pool;
		}

		public static void Init(Pool2 *self) {
			Init(self, 64);
		}
		public static void Init(Pool2 *self, int len) {
			if (!Initialized) TypeInit();
			#if FDB
			Should.NotNull("self", self);
			Should.GreaterThanZero("len", len);
			self->type = Type;
			#endif
			self->len = len;
			int headSize = HeadSize;
			int tailSize = TailSize;
			byte *arr = self->arr = (byte *)Mem.Malloc(len);
			self->shift = 0;

			// sentinel
			int *head = (int *)arr;
			head[0] = -1;  // sentinelHead.prev = -1;
			head[1] = headSize + tailSize;  // sentinelhead.next = headSize + tailSize;
			head[2] = 0;  // sentinelHead.size = 0;
			head[3] = -1;  // sentinelTail.size = -1;  // to prevent merging
//			Fdb.Dump(arr, len);
			//               sentinel              firstFree             sentinel
			int size = len - headSize - tailSize - headSize - tailSize - headSize;
			SetFreeMeta((int *)(arr + headSize + tailSize), 0, -1, size);

			// sentinel
			head = (int *)(arr + len - headSize);
			head[0] = -1;  // prev = -1 will prevent merging
			head[1] = -1;
			head[2] = 0;
			#if FDB
			Verify(self);
			#endif
		}

		/**
		 * | -1 | -1 | size | data... | -1 |
		 *                  | size    |
		 */
		static void SetUsedMeta(int *head, int size) {
			#if FDB
			Should.NotNull("head", head);
			Should.GreaterThanZero("size", size);
			#endif
			head[0] = -1; head[1] = -1; head[2] = size;
			int *tail = (int *)((byte *)(head + 3) + size);
			tail[0] = -1;
		}

		/**
		 * | prev | next | size | data... | size |
		 *                      | size    |
		 */
		static void SetFreeMeta(int *head, int prev, int next, int size) {
			#if FDB
			Should.NotNull("head", head);
			Should.GreaterThanOrEqualTo("prev", prev, -1);
			Should.GreaterThanOrEqualTo("next", next, -1);
			Should.GreaterThanZero("size", size);
			#endif
			head[0] = prev; head[1] = next; head[2] = size;
			int *tail = (int *)((byte *)(head + 3) + size);
			tail[0] = size;
		}

		/**
		 * | prev | next | size | data... | size |
		 *                      | size    |
		 */
		static void SetFreeMeta(int *head, int size) {
			#if FDB
			Should.NotNull("head", head);
			Should.GreaterThanZero("size", size);
			#endif
			head[2] = size;
			int *tail = (int *)((byte *)(head + 3) + size);
			tail[0] = size;
		}

		/**
		 * | 0 | firstFreeHead.next | size | data... | size |
		 *                                 | size    |
		 */
		static void SetFreeMetaAndInsert(byte *arr, int *head, int size) {
			#if FDB
			Should.NotNull("arr", arr);
			Should.NotNull("head", head);
			Should.GreaterThanZero("size", size);
			#endif
			int *firstFreeHead = (int *)arr;
			int firstFreeHeadNext;
			head[0] = 0; firstFreeHeadNext = head[1] = firstFreeHead[1]; head[2] = size;

			int pos = (int)((byte*)head - arr);
			if (firstFreeHeadNext != -1) *(int *)(arr + firstFreeHeadNext) = pos;  // firstFreeHead.next.prev = head
			firstFreeHead[1] = pos;  // firstFreeHead.next = head
			int *tail = (int *)((byte *)(head + 3) + size);
			tail[0] = size;
		}

		static void RemoveFromFreeList(byte *arr, int *head) {
			#if FDB
			Should.NotNull("arr", arr);
			Should.NotNull("head", head);
			#endif
			int prev = head[0], next = head[1];
			if (prev != -1) ((int *)(arr + prev))[1] = next;
			if (next != -1) ((int *)(arr + next))[0] = prev;
		}

		/**
		 * | head    | data... | tail | rightHead | data...   | rightTail |
		 *           | size    |                  | rightSize |
		 * | newHead | data...                                | tail      |
		 *           | newSize                                |
		 * assume head is free and linked
		 */
		static void MergeRight(byte *arr, int *head) {
			#if FDB
			Should.NotNull("arr", arr);
			Should.NotNull("head", head);
			#endif
			int headSize = HeadSize, tailSize = TailSize;
			int size = head[2];
			int *rightHead = (int *)((byte *)head + headSize + size + tailSize);
			if (rightHead[0] == -1) return;  // since only the left most sentinel has head.prev = -1, we can check this to see if the node is used

			int newSize = size;
			do {
				int rightSize = rightHead[2];
				newSize = newSize + tailSize + headSize + rightSize;
				RemoveFromFreeList(arr, rightHead);

				rightHead = (int *)((byte *)rightHead + headSize + rightSize + tailSize);
			} while (rightHead[0] != -1);

			SetFreeMeta(head, newSize);  // do not need to insert since head is already in free list
		}

		/**
		 * | leftHead | data...  | leftTail | head | data... | tail    |
		 *            | leftSize |                 | size    |
		 * | newHead  | data...                              | newTail |
		 *            | newSize                              |
		 * assume the head is free and linked
		 */
		static int *MergeLeft(byte *arr, int *head) {
			#if FDB
			Should.NotNull("arr", arr);
			Should.NotNull("head", head);
			#endif
			int headSize = HeadSize, tailSize = TailSize;
			int *leftTail = head - 1;
			int leftSize = *leftTail;
			if (leftSize == -1) return head;

			RemoveFromFreeList(arr, head);
			int *leftHead;
			int newSize = head[2];
			do {
				leftHead = (int *)((byte *)leftTail - leftSize - headSize);
				RemoveFromFreeList(arr, leftHead);
				newSize = leftSize + tailSize + headSize + newSize;

				leftTail = leftHead - 1;
				leftSize = *leftTail;
			} while (leftSize != -1);

			SetFreeMetaAndInsert(arr, leftHead, newSize);
			return leftHead;
		}

		public static void *Alloc(Pool2 *self, int size) {
			#if FDB
			Verify(self);
			Should.GreaterThanZero("size", size);
			#endif
			if ((size & 0x3) != 0) {  // align to 4
				size = (((size >> 2) + 1) << 2);
			}
			int headSize = HeadSize, tailSize = TailSize;
			byte *arr = self->arr;

			int *head;
			int *freeHead;
			int freeSize;

			int curFree = 0;
			while (curFree != -1) {  // foreach free block
				head = (int *)(arr + curFree);
				int blockSize = head[2];
				if (blockSize >= size) {  // found a block that can fit
					// | head | data...                              | tail |
					//        | blockSize                            |
					// | head | data... | tail | freeHead | data...  | tail |
					//        | size    |                 | freeSize |
					freeSize = blockSize - size - tailSize - headSize;
					if (freeSize > 0) {  // split
						freeHead = (int *)((byte *)head + headSize + size + tailSize);
						SetFreeMetaAndInsert(arr, freeHead, freeSize);
						MergeRight(arr, freeHead);

						RemoveFromFreeList(arr, head);
						SetUsedMeta(head, size);
					} else {
						RemoveFromFreeList(arr, head);
						SetUsedMeta(head, blockSize);
					}
					#if FDB
					Verify(self);
					#endif
					return head + 3;
				}
				curFree = head[1];
			}

			// expand and split
			// | data... | endHead  |
			// | data... | freeHead | data...  | freeTail | head | data... | tail | endHead |
			// | len                                                                        |
			// | oldLen             | freeSize |                 | size    |
			int oldLen = self->len, len = oldLen, minLen = oldLen + headSize + size + tailSize + headSize + tailSize;
			while (len < minLen) len <<= 1;
			self->len = len;
//			Fdb.Log("{0:X}", (long)arr);
//			Fdb.Log("len: {0}", Mem.Verify(arr));
			byte *oldArr = arr;
			arr = self->arr = (byte *)Mem.Realloc(arr, len);
			self->shift = arr - oldArr;
//			Fdb.Log("{0:X}", (long)arr);
//			Fdb.Log("len: {0}", Mem.Verify(self->arr));


			freeHead = (int *)(arr + oldLen - headSize);
			freeSize = len - oldLen - tailSize - headSize - size - tailSize - headSize;
			SetFreeMetaAndInsert(arr, freeHead, freeSize);
//			Fdb.Dump(arr, len);
			MergeLeft(arr, freeHead);

			int *endHead = (int *)(arr + len - headSize);
			endHead[0] = -1;
			endHead[1] = -1;
			endHead[2] = 0;

			head = (int *)((byte *)freeHead + headSize + freeSize + tailSize);
			SetUsedMeta(head, size);
			#if FDB
			Verify(self);
			#endif
			return head + 3;
		}

		public static void Free(Pool2 *self, void *ptr) {
			#if FDB
			Verify(self);
			Should.InRange("ptr", ptr, self->arr, self->arr + self->len);
			#endif
			byte *arr = self->arr;
//			Fdb.Dump(arr, self->len);

			int *head = (int *)ptr - 3;
			SetFreeMetaAndInsert(arr, head, head[2]);
			#if FDB
			Verify(self);
			#endif
			MergeRight(arr, head);
			#if FDB
			Verify(self);
			#endif
			MergeLeft(arr, head);
			#if FDB
			Verify(self);
			#endif
		}

		public static void Clear(Pool2 *self) {
			int len = self->len;
			int headSize = HeadSize;
			int tailSize = TailSize;
			byte *arr = self->arr;

			// sentinel
			int *head = (int *)arr;
			head[0] = -1;  // sentinelHead.prev = -1;
			head[1] = headSize + tailSize;  // sentinelhead.next = headSize + tailSize;
			head[2] = 0;  // sentinelHead.size = 0;
			head[3] = -1;  // sentinelTail.size = -1;  // to prevent merging
			//			Fdb.Dump(arr, len);
			//               sentinel              firstFree             sentinel
			int size = len - headSize - tailSize - headSize - tailSize - headSize;
			SetFreeMeta((int *)(arr + headSize + tailSize), 0, -1, size);

			// sentinel
			head = (int *)(arr + len - headSize);
			head[0] = -1;  // prev = -1 will prevent merging
			head[1] = -1;
			head[2] = 0;
		}

		#if FDB
		public static void Verify(Pool2 *self) {
			Should.NotNull("self", self);
			Should.TypeEqual("self", self->type, Type);

			int headSize = HeadSize, tailSize = TailSize;
			Should.GreaterThanZero("self->len", self->len);
			Should.Equal("self->headSize", headSize, 3 * sizeof(int));
			Should.Equal("self->tailSize", tailSize, sizeof(int));

			int len = Mem.Verify(self->arr);
			Should.Equal("self->len", self->len, len);

//			Fdb.Dump(self->arr, len);

			byte *arr = self->arr;
			int *head;
			head = (int *)(arr + self->len - headSize);
			Should.Equal("endHead->prev", head[0], -1);  // head.prev = -1
			Should.Equal("endHead->next", head[1], -1);  // head.next = -1
			Should.Equal("endHead->size", head[2], 0);  // head.size = 0

			head = (int *)arr;
			Should.Equal("firstHead->prev", head[0], -1);  // head.prev = -1
			Should.Equal("firstHead->size", head[2], 0);  // head.size = 0
			Should.Equal("firstTail->size", head[3], -1);  // tail.size = -1

			int curFree = head[1], lastFree = 0;
			var dict = new System.Collections.Generic.Dictionary<int, int>();
			while (curFree != -1) {
				head = (int *)(arr + curFree);
				Should.Equal("head" + curFree + "->prev", head[0], lastFree);
				Should.Equal("tail->size", *(int *)((byte *)head + headSize + head[2]), head[2]);
				dict.Add(curFree, head[2]);
				lastFree = curFree;
				curFree = head[1];
			}

			head = (int *)(arr + headSize + tailSize);
			int *end = (int *)(arr + self->len);
			while (head < end) {
				int pos = (int)((byte *)head - self->arr);
				int prev = head[0], next = head[1], size = head[2];
				int *tail = (int *)((byte *)head + headSize + size);

				if (tail < end) {
					int tailVal = tail[0];
					if (tailVal == -1) {  // in use
						Should.False("dict.ContainsKey(pos)", dict.ContainsKey(pos));
						Should.Equal("prev", prev, -1);
						Should.Equal("next", next, -1);
						Should.GreaterThanZero("size", size);
					} else {  // free
						Should.True("dict.ContainsKey(pos)" + pos, dict.ContainsKey(pos));
						Should.GreaterThanOrEqualTo("prev", prev, 0);
						Should.GreaterThanOrEqualTo("next", next, -1);
						Should.Equal("size", size, dict[pos]);
						dict.Remove(pos);
					}
				} else {  // head is end sentinel, no tail
					Should.Equal("head", head, (int *)(arr + len - headSize));
					Should.Equal("endHead->prev", head[0], -1);  // head.prev = -1
					Should.Equal("endHead->next", head[1], -1);  // head.next = -1
					Should.Equal("endHead->size", head[2], 0);  // head.size = 0
				}

				head = (int *)((byte *)head + headSize + size + tailSize);
			}
			Should.Zero("dict.Count", dict.Count);
		}

		public static void Test() {
			TypeInit();
			TestExpand1();
			TestRandomExpand();
			TestRandomAllocFree();
		}

		static void TestExpand1() {
			var pool = stackalloc Pool2[1]; Init(pool);
			var p = Pool2.Alloc(pool, 4);
			*(int *)p = 0x4a4a4a4a;
			Pool2.Free(pool, p);
			p = Pool2.Alloc(pool, 4);
			*(int *)p = 0x4a4a4a4a;
			var p2 = Pool2.Alloc(pool, 4);
			*(int *)p2 = 0x4a4a4a4a;
			Pool2.Free(pool, p2);
			p = (void *)((byte *)p + pool->shift);
			Pool2.Free(pool, p);
			// the first *real* free node should be 0x54 long (containing all space)
			Should.Equal("*(int *)(pool->arr + 0x28)", *(int *)(pool->arr + 0x18), pool->len - 11 * 4);
		}

		static void TestRandomExpand() {
			var pool = stackalloc Pool2[1]; Init(pool);
			const int len = 200;
			var ptrs = stackalloc byte *[len];
			for (int i = 0; i < len; i += 1) {
				int size = Fdb.Random(1, 1000);
				ptrs[i] = (byte *)Pool2.Alloc(pool, size);
				if (pool->shift != 0) {
					long shift = pool->shift;
					for (int j = 0; j < i; j += 1) {
						ptrs[j] += shift;
					}
					pool->shift = 0;
				}
			}
			for (int i = 0; i < len; i += 1) {
				Pool2.Free(pool, ptrs[i]);
			}
			// the first *real* free node should be 0x54 long (containing all space)
			Should.Equal("*(int *)(pool->arr + 0x28)", *(int *)(pool->arr + 0x18), pool->len - 11 * 4);
		}



		static void TestRandomAllocFree() {
			var pool = stackalloc Pool2[1]; Init(pool);
			const int len = 200;
			var ptrs = new System.Collections.Generic.List<long>();
			for (int i = 0; i < len; i += 1) {
				int size = Fdb.Random(1, 1000);
				ptrs.Add((long)Pool2.Alloc(pool, size));
				if (pool->shift != 0) {
					long shift = pool->shift;
					for (int j = 0; j < ptrs.Count - 1; j += 1) {
						ptrs[j] += shift;
					}
					pool->shift = 0;
				}
				if (Fdb.Random(0, 2) == 0) {  // 1/3 chance to free
					int idx = Fdb.Random(0, ptrs.Count - 1);
					Pool2.Free(pool, (void *)ptrs[idx]);
					ptrs.RemoveAt(idx);
				}
			}
			for (int i = 0; i < ptrs.Count; i += 1) {
				Pool2.Free(pool, (void *)ptrs[i]);
			}
			// the first *real* free node should be 0x54 long (containing all space)
			Should.Equal("*(int *)(pool->arr + 0x28)", *(int *)(pool->arr + 0x18), pool->len - 11 * 4);
		}
		#endif
	}
}

