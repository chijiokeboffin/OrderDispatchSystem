using BlueCorpOrder.Application.AppInsightConfiguration;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Infrastructure.ThreePLServiceIntegration
{
    public class ThreePLService: IThreePLService
    {
        private readonly IInsightsTracker _insightsTracker;
        public ThreePLService(IInsightsTracker insightsTracker)
        {
            _insightsTracker = insightsTracker;
        }
        public async Task<bool> UploadFileToSftpAsync(string csvDataString)
        {
            var sftpHost = Environment.GetEnvironmentVariable("SFTP_HOST");
            var sftpUsername = Environment.GetEnvironmentVariable("SFTP_USERNAME");
            var sftpPrivateKeyPath = Environment.GetEnvironmentVariable("SFTP_PRIVATE_KEY_PATH");
            try
            {
                var privateKey = new PrivateKeyFile(sftpPrivateKeyPath);
                var connectionInfo = new ConnectionInfo(sftpHost, sftpUsername, new PrivateKeyAuthenticationMethod(sftpUsername, privateKey));

                using (var sftp = new SftpClient(connectionInfo))
                {
                    sftp.Connect();
                    var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvDataString));
                    sftp.UploadFile(stream, "/bluecorp-incoming/dispatch-request.csv");
                    sftp.Disconnect();
                }

                _insightsTracker.TrackEventToInsights(nameof(UploadFileToSftpAsync), csvDataString, "CSV file uploaded successfully to SFTP.");
                return true;
            }
            catch (Exception ex)
            {
                _insightsTracker.TrackException(ex, $"Failed to upload CSV to SFTP: {ex.Message}");
                return false;
            }
        }
    }
}
