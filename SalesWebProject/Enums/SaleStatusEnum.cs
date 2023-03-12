using System.ComponentModel;

namespace SalesWebProject.Enums
{
    public enum SaleStatusEnum
    {
        [Description("Pendente")]
        Pending = 1,

        [Description("Pago")]
        Billed = 2,

        [Description("Cancelado")]
        Canceled = 3
    }
}
