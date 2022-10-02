
public class InputSystemProvider : MonoSingleton<InputSystemProvider>
{
    public override void Init()
    {
        base.Init();
        this.battlePlayerInputSystemPrivate = new BattleInput();
    }
    /// <summary>
    /// Система ввода во время боя.
    /// </summary>
    private BattleInput battlePlayerInputSystemPrivate;
    /// <summary>
    /// Система ввода во время боя.
    /// </summary>
    public BattleInput battlePlayerInputSystem => this.battlePlayerInputSystemPrivate;
}

