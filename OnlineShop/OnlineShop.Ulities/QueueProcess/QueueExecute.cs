using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OnlineShop.QueueProcess
{
    public class QueueElement<T>
    {
        public T target { get; set; }
        public int repeatLeft { get; set; }
        public DateTime nextTime { get; set; }
        public int priority { get; set; }
        public int uid { get; set; }

        public QueueElement(T cmdobj, int repeatNum, int uniqueid = -1, int priorityVal = -1)
        {
            target = cmdobj;
            repeatLeft = repeatNum;
            nextTime = DateTime.MinValue;
            priority = priorityVal;
            uid = uniqueid;
        }
    }

    public class QueueExecute<T>
    {
        private bool bLoop = false;
        private bool bRepeatLoop = false;
        private Thread? th = null;
        private Thread? repeatth = null;
        private ManualResetEvent? mre = null;
        private ManualResetEvent? repeatmre = null;
        private LinkedList<QueueElement<T>>? queue = null;
        private LinkedList<QueueElement<T>>? repeatqueue = null;
        private int repeatNum;
        private int interval;
        public bool continuousRepeat;   // true:エレメントを連続して繰り返す、false:他のエレメントがあればそれを処理をしてから繰り返す

        // msIntervalは、連続(continuous:true)の場合は処理が終わってから次までの時間
        //               連続では無い(continuous:false)場合は、処理開始から次までの時間
        public QueueExecute(int numRepeat, int msInterval, bool continuous)
        {
            repeatmre = new ManualResetEvent(false);
            mre = new ManualResetEvent(false);
            queue = new LinkedList<QueueElement<T>>();
            repeatqueue = new LinkedList<QueueElement<T>>();
            repeatNum = numRepeat;
            interval = msInterval;
            continuousRepeat = continuous;
        }

        public String DebugShowList()
        {
            StringBuilder? str = null;
            lock (((ICollection)repeatqueue!).SyncRoot)
            {
                lock (((ICollection)queue!).SyncRoot)
                {
                    str = new StringBuilder((repeatqueue.Count + queue.Count) * 16 + 1024);
                    str.Append("DebugShowList() Start\r\n");
                    str.Append("***queue.Count: " + queue.Count.ToString() + "\r\n");
                    LinkedListNode<QueueElement<T>>? node = null;
                    int iCnt = 1;
                    for (node = queue.First; node != null; iCnt++, node = node.Next)
                    {
                        str.Append($"{iCnt:00000}: uid:{node.Value.uid:00000} priority:{node.Value.priority:000}\r\n");
                    }

                    str.Append("***repeatqueue.Count: " + repeatqueue.Count.ToString() + "\r\n");
                    iCnt = 1;
                    for (node = repeatqueue.First; node != null; iCnt++, node = node.Next)
                    {
                        str.Append($"{iCnt:00000}: uid:{node.Value.uid:00000} priority:{node.Value.priority:000}\r\n");
                    }
                    str.Append("DebugShowList() End\r\n");
                }
            }

            if (null != str)
                return str.ToString();

            return "";
        }

        // uid = -1: all
        public void QueueClear(int uid = -1)
        {
            RepeatQueueClear(uid);
            QueueClear(queue!, uid);
        }

        // uid = -1: all
        public void RepeatQueueClear(int uid = -1)
        {
            QueueClear(repeatqueue!, uid);
        }

        private void QueueClear(LinkedList<QueueElement<T>> que, int uid = -1, bool all = true)
        {
            lock (((ICollection)que).SyncRoot)
            {
                if (-1 == uid && true == all)
                {
                    que.Clear();
                }
                else
                {
                    LinkedListNode<QueueElement<T>>? node = null;
                    for (node = queue!.First; node != null;)
                    {
                        LinkedListNode<QueueElement<T>>? tmpnode = node.Next;
                        if (node.Value.uid == uid)
                        {
                            queue.Remove(node);
                            if (false == all)
                                break;
                        }
                        node = tmpnode;
                    }
                }
            }
        }

        // 同じuidを持つelementがキューにあった時の処理
        public enum UNIQUEMETHOD
        {
            NONE,       // 何もしない（重複許可、priorityに従う）
            REPLACE,    // 置き換える（priorityより優先される）
            REMOVE,     // 取り除いてから追加（取り除いた後はpriorityに従う）
        }

        // 優先順位が自身より低いものがあったらその前に挿入する
        // なければ最後尾に追加する
        public void QueueAdd(T obj, UNIQUEMETHOD mode = UNIQUEMETHOD.NONE, int uniqueid = -1, int priorityVal = -1, bool all = true)
        {
            QueueElement<T> newElement = new QueueElement<T>(obj, repeatNum, uniqueid, priorityVal);
            QueueAdd(newElement, mode, all);
        }

        private void QueueAdd(QueueElement<T> element, UNIQUEMETHOD mode = UNIQUEMETHOD.NONE, bool all = true)
        {
            lock (((ICollection)queue!).SyncRoot)
            {
                if (UNIQUEMETHOD.NONE != mode && -1 != element.uid)
                {
                    LinkedListNode<QueueElement<T>>?node = null;
                    for (node = queue.First; node != null;)
                    {
                        LinkedListNode<QueueElement<T>>? tmpnode = node.Next;
                        if (node.Value.uid == element.uid)
                        {
                            if (UNIQUEMETHOD.REPLACE == mode)
                            {
                                node.Value = element;
                                if (false == all)
                                    return;
                            }
                            else if (UNIQUEMETHOD.REMOVE == mode)
                            {
                                queue.Remove(node);
                                if (false == all)
                                    break;
                            }
                        }
                        node = tmpnode;
                    }
                    // 繰り返し用のキューからは取り除く
                    RepeatQueueClear(element.uid);
                }
                if (element.priority != -1 && queue.Count > 0)
                {
                    LinkedListNode<QueueElement<T>>? node = null;
                    for (node = queue.First; node != null; node = node.Next)
                    {
                        if (node.Value.priority > element.priority)
                        {
                            queue.AddBefore(node, element);
                            mre!.Set();
                            return;
                        }
                    }
                }
                queue.AddLast(element);
                mre!.Set();
            }
        }

        private void QueueAddFirst(QueueElement<T> obj)
        {
            lock (((ICollection)queue!).SyncRoot)
            {
                queue.AddFirst(obj);
                mre!.Set();
            }
        }

        private void RepeatQueueAdd(QueueElement<T> obj)
        {
            lock (((ICollection)repeatqueue!).SyncRoot)
            {
                repeatqueue.AddLast(obj);
                // 再送キューに今入れた物しか無い場合にイベント発火
                if (repeatqueue.Count == 1)
                    repeatmre!.Set();
            }
        }

        public void RepeatQueueAddFirst(QueueElement<T> obj)
        {
            lock (((ICollection)repeatqueue!).SyncRoot)
            {
                repeatqueue.AddFirst(obj);
                // 再送キューに今入れた物しか無い場合にイベント発火
                if (repeatqueue.Count == 1)
                    repeatmre!.Set();
            }

        }

        public void Start()
        {
            if (repeatth == null)
            {
                bRepeatLoop = true;
                repeatth = new Thread(new ThreadStart(RepeatObserve));
                repeatmre!.Reset();
                repeatth.Start();
            }

            if (th == null)
            {
                bLoop = true;
                th = new Thread(new ThreadStart(Observe));
                mre!.Reset();
                th.Start();
            }
        }

        public void Stop()
        {
            if (null != repeatth)
            {
                lock (((ICollection)repeatqueue!).SyncRoot)
                {
                    bRepeatLoop = false;
                    repeatmre!.Set();
                }
                while (true)
                {
                    if (false == repeatth.IsAlive)
                    {
                        repeatth = null;
                        break;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }

            if (null != th)
            {
                lock (((ICollection)queue!).SyncRoot)
                {
                    bLoop = false;
                    mre!.Set();
                }
                while (true)
                {
                    if (false == th.IsAlive)
                    {
                        th = null;
                        break;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }

        // interval間隔での繰り返しは、あくまでキューに入った順に実行キューへ戻す
        public void RepeatObserve()
        {
            while (bLoop)
            {
                bool execontinue = true;
                QueueElement<T>? cmd = null;
                repeatmre!.WaitOne();
                while (execontinue)
                {
                    lock (((ICollection)repeatqueue!).SyncRoot)
                    {
                        if (bRepeatLoop == false)
                            return;
                        if (repeatqueue.Count != 0)
                        {
                            // 再送用のキューの先頭を取って来るが削除しない
                            cmd = repeatqueue.First?.Value;
                        }
                        mre!.Reset();
                    }
                    if (cmd != null)
                    {
                        DateTime dtNow = DateTime.Now;
                        TimeSpan ts = cmd.nextTime - dtNow;
                        if (ts.TotalMilliseconds > 0)
                        {
                            // 時間が来る（タイムアウトする）まで待つ（次回のループで実行される）
                            // Stop時もWaitから抜ける。
                            repeatmre.WaitOne((int)ts.TotalMilliseconds);
                        }
                        else
                        {
                            // ここで再送用のキューから削除
                            lock (((ICollection)repeatqueue).SyncRoot)
                            {
                                repeatqueue.RemoveFirst();
                            }
                            // コマンドを実行用キューに格納
                            QueueAdd(cmd);
                        }
                    }
                    lock (((ICollection)repeatqueue).SyncRoot)
                    {
                        if (repeatqueue.Count == 0)
                            execontinue = false;
                    }
                }
            }
        }

        public void Observe()
        {
            while (bLoop)
            {
                bool execontinue = true;
                QueueElement<T>? cmd = null;
                mre!.WaitOne();
                while (execontinue)
                {
                    lock (((ICollection)queue!).SyncRoot)
                    {
                        if (bLoop == false)
                            return;
                        if (queue.Count != 0)
                        {
                            cmd = queue.First?.Value;
                            queue.RemoveFirst();
                        }
                        mre.Reset();
                    }
                    if (cmd != null)
                    {
                        // 初回実行時に現在時刻をセット
                        if (cmd.nextTime == DateTime.MinValue)
                        {
                            cmd.nextTime = DateTime.Now;
                        }
                        else if (true == continuousRepeat)
                        {
                            // 連続繰り返し実行なら
                            DateTime dtNow = DateTime.Now;
                            TimeSpan ts = cmd.nextTime - dtNow;
                            if (ts.TotalMilliseconds > 0)
                            {
                                // 時間が来る（タイムアウトする）まで待つ
                                // Stop時もWaitから抜ける。
                                mre.WaitOne((int)ts.TotalMilliseconds);
                                lock (((ICollection)queue).SyncRoot)
                                {
                                    if (bLoop == false)
                                        return;
                                }
                            }
                        }
                        DoCommand(cmd);
                    }
                    if (cmd!.repeatLeft > 0)
                    {
                        // 再送カウントが0でないならカウントをデクリメントして、次回実行時間をセットし
                        // 再送用のキューに格納
                        // intervalは、連続の場合は処理が終わってから次までの時間
                        //             連続では無い場合は、処理開始から次までの時間
                        cmd.repeatLeft--;
                        if (true == continuousRepeat)
                            cmd.nextTime = DateTime.Now;
                        cmd.nextTime = cmd.nextTime.AddMilliseconds(interval);
                        if (true == continuousRepeat)
                            QueueAddFirst(cmd);  // 連続繰り返し実行なら
                        else
                            RepeatQueueAdd(cmd);
                    }

                    lock (((ICollection)queue).SyncRoot)
                    {
                        if (queue.Count == 0)
                            execontinue = false;
                    }
                    if (execontinue == true)
                        Thread.Sleep(20);
                }
            }
        }

        public virtual void DoCommand(QueueElement<T> cmd)
        {
            //do something
            // cmd.obj.xxx()
        }
    }
}