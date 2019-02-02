namespace BBRAddin.Model
{
    public class MenuCommandParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MenuType MenuItemType { get; set; }
    }

    public enum MenuType
    {
        Table,
        View,
        Sp,
        Function,
        Manage,
        Fk,
        Pk
    }
}
