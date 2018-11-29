﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Events;

namespace ATMHandin3.Interfaces
{
    public interface IFileLogger
    {
        bool CheckIfFileExists();
        void ReceiverWarningRaised(object sender, SeparationEventArgs e);

    }
}
