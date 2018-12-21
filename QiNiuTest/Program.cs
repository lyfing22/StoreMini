using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Util;
using Qiniu.IO.Model;
using Qiniu.IO;
using Qiniu.Http;

namespace QiNiuTest
{
    class Program
    {
        static void Main(string[] args)
        {
           
            // Qiniu.Common.Config.SetZone(Qiniu.Common.ZoneID.CN_East, true);
            Mac mac = new Mac("y27q-mmNDCnGxjdT-Y7HZ87AFzAwk7ibXo086uK2", 
                "ARDhJKGhWklqGq40iElSSvkclP11h7ZLcZU2nctd");
            string bucket = "zhangshangzu";
            string saveKey = "girl/gf/a.jpg";
            string localFile = "D:\\1.jpg";

            Qiniu.Common.Config.AutoZone("y27q-mmNDCnGxjdT-Y7HZ87AFzAwk7ibXo086uK2",
                bucket, true);
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            UploadManager um = new UploadManager();
            HttpResult result = um.UploadFile(localFile, saveKey, token);
            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
