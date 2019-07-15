namespace LaBataille.Tests
{
    internal class NullGameOverVisitor : IVisitGameOver
    {
        public static IVisitGameOver Instance = new NullGameOverVisitor();

        private NullGameOverVisitor(){ }
        
        public void Visit(Game game, IAmTheGameOver gameOver)
        {
        }
    }
}