using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League.Domein.Domein;

namespace League.Domein
{
    public interface ITransferRepository
    {
        Transfer SchrijfTransferInDB(Transfer transfer);
    }
}
