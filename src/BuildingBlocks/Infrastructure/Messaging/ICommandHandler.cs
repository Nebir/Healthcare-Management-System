using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BuildingBlocks.Infrastructure.Messaging
{
    public interface ICommandHandler
    {
        void Handle(ICommand command);
    }
}
