using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.QueueProcess
{
    /// <summary>
    /// how to use:
    /// SoundQueue.QueueClear();
    /// SoundQueue.Start();
    /// SoundQueue.Add()();
    /// SoundQueue.Stop();
    /// </summary>
    public class SoundElement
    {
        public string msg { get; set; }
    }

    public class SoundQueue : QueueExecute<SoundElement>
    {
        private static readonly log4net.ILog _logger
           = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public SoundQueue(int numRepeat, int msInterval, bool continuous) : base(numRepeat, msInterval, continuous)
        {; }

        public override void DoCommand(QueueElement<SoundElement> cmd)
        {
            _logger.Info(cmd.target.msg);
            Thread.Sleep(10 * 1000);
        }
    }
}
