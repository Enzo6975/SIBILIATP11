

namespace SIBILIATP11.Classe
{
    public interface ICrud<T>
   {
        public int Create();

        public void Read();

        public int Update();

        public int Delete();
      
        public List<T> FindAll();

        public List<T> FindBySelection(string criteres);
   
   }
}