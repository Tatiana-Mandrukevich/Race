namespace DefaultNamespace
{
    public class Chunk : MonoPooled
    {
        private TrafficCone[] _allCones;

        private void Awake()
        {
            // Находим все конусы один раз при создании чанка
            _allCones = GetComponentsInChildren<TrafficCone>(true);
        }

        public override void Initialize()
        {
            base.Initialize();
            
            // Сбрасываем все конусы, используя сохраненный список, 
            // так как сбитые конусы могут быть не в иерархии в этот момент
            if (_allCones != null)
            {
                foreach (var cone in _allCones)
                {
                    if (cone != null) cone.ResetCone();
                }
            }
        }
    }
}