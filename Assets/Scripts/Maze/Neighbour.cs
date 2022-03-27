namespace Maze
{
    public class Neighbour
    {
        public readonly Position Coordinates;
        public readonly SharedWall Wall;

        public Neighbour(int x, int y, SharedWall wall)
        {
            Coordinates = new Position(x, y);
            Wall = wall;
        }
    }
}
