public class ItemsLayout : LayoutBase
{
    public void SelectItem(int towerNumber)
    {
        DefenseController.Instance.CreateManager.SelectTower(towerNumber);
    }
}
