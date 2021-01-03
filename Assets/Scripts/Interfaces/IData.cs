using System.Collections.Generic;

namespace Labyrinth
{
    public interface IData<T>
    {
        void Save(T data, string path = null);

        T Load(string path = null);
    }

    public interface IData
    {
        IData Save(List<SavedData> data, string path = null);

        void Save(Seed data, string path = null);

        List<SavedData> Load(string path = null);
        
        Seed Load(int a, string path = null);
    }
}
