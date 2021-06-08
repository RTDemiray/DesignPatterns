using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Commands
{
    public interface ITableActionCommand
    {
        IActionResult Execute();
    }
}