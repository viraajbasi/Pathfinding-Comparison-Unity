namespace Maze
{
    public class Neighbour
    {
        public Position Coordinates;
        public SharedWall Wall;

        public Neighbour(int x, int y, SharedWall wall)
        {
            Coordinates = new Position(x, y);
            Wall = wall;
        }
    }
}
