using UnityEngine;
using static UnityEngine.Debug;

namespace Observers
{
    public sealed class DelegatesObserver
    {
        public delegate void MyDelegate(object o);
        public sealed class Sourse
        {
            private MyDelegate _functions;

            public void Add(MyDelegate f)
            { _functions += f; }

            public void Remove(MyDelegate f)
            { _functions -= f; }

            public void Run()
            {
                Log("Run!");
                if (_functions != null) _functions(this);
            }
        }

        public sealed class Observer1
        {
            public void Do(object o)
            { Log($"Первый принял, что объект {o} побежал"); }
        }
        public sealed class Observer2
        {
            public void Do(object o)
            { Log($"Второй принял, что объект {o} побежал"); }
        }
    }

    public class TestObservers : MonoBehaviour
    {
        private void Start()
        {
            DelegatesObserver.Sourse s = new DelegatesObserver.Sourse();
            DelegatesObserver.Observer1 o1 = new DelegatesObserver.Observer1();
            DelegatesObserver.Observer2 o2 = new DelegatesObserver.Observer2();
            DelegatesObserver.MyDelegate d1 = new DelegatesObserver.MyDelegate(o1.Do);
            s.Add(d1);
            s.Add(o2.Do);
            s.Run();
            s.Remove(o1.Do);
            s.Run();
        }
    }
}