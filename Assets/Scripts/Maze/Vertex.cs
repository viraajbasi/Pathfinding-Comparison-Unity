namespace Maze
{
    public class Vertex
    {
        public string Name { get; set; }
        public string Distance { get; set; }
        public Vertex Parent { get; set; }

        public Vertex(string distance, Vertex parent)
        {
            Distance = distance;
            Parent = parent;
        }
    }
}
