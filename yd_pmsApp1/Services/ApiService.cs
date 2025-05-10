using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

public static class ApiService
{
    private static readonly HttpClient client;

    static ApiService()
    {
        var proxy = new WebProxy()
        {
            Address = new Uri("http://120.26.136.102/mms_sm_req"), // 👈 换成你的代理地址
            BypassProxyOnLocal = false,
            UseDefaultCredentials = false,
            // Credentials = new NetworkCredential("用户名", "密码") // 如果需要账号密码解开
        };

        var httpClientHandler = new HttpClientHandler()
        {
            Proxy = proxy,
            UseProxy = true
        };

        client = new HttpClient(httpClientHandler);
    }

    public static async Task<string> LoginAsync(string account, string password)
    {
        string encryptedPassword = AESEncryption.Encrypt(password);

        var requestData = new
        {
            REQUEST = new
            {
                HDR = new
                {
                    SERVICE_ID = "10000",
                    OTYPE = 1
                },
                DATA = new
                {
                    ACCOUNT = account,
                    PASSWORD = encryptedPassword
                }
            }
        };

        string jsonContent = JsonConvert.SerializeObject(requestData);

        // ✅ 打印格式化后的请求数据
        Debug.WriteLine("=== 请求数据 ===");
        Debug.WriteLine(FormatJson(jsonContent));
        Debug.WriteLine("================");

        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("http://120.26.136.102/mms_sm_req", httpContent);

        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        // ✅ 打印格式化后的响应数据
        Debug.WriteLine("=== 应答数据 ===");
        Debug.WriteLine(FormatJson(responseBody));
        Debug.WriteLine("================");

        return responseBody;
    }

    public static async Task<string> LogoutAsync(string userId, string session)
    {
        var requestData = new
        {
            REQUEST = new
            {
                HDR = new
                {
                    SERVICE_ID = "10000",
                    OTYPE = 2,
                    USER_ID = userId,
                    SESSION = session
                },
                DATA = new { }
            }
        };

        string jsonContent = JsonConvert.SerializeObject(requestData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // 打印格式化后的请求数据
        Debug.WriteLine("=== 注销请求数据 ===");
        Debug.WriteLine(FormatJson(jsonContent));
        Debug.WriteLine("================");

        HttpResponseMessage response = await client.PostAsync("http://120.26.136.102/mms_sm_req", httpContent);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        // 打印格式化后的响应数据
        Debug.WriteLine("=== 注销应答数据 ===");
        Debug.WriteLine(FormatJson(responseBody));
        Debug.WriteLine("================");

        return responseBody;
    }

    public static async Task<string> ChangePasswordAsync(string userId, string session, string oldPassword, string newPassword)
    {
        string encryptedOldPassword = AESEncryption.Encrypt(oldPassword);
        string encryptedNewPassword = AESEncryption.Encrypt(newPassword);

        var requestData = new
        {
            REQUEST = new
            {
                HDR = new
                {
                    SERVICE_ID = "10000",
                    OTYPE = 3,
                    USER_ID = userId,
                    SESSION = session
                },
                DATA = new
                {
                    OLD_PASSWORD = encryptedOldPassword,
                    NEW_PASSWORD = encryptedNewPassword
                }
            }
        };

        string jsonContent = JsonConvert.SerializeObject(requestData);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // 打印格式化后的请求数据
        Debug.WriteLine("=== 修改密码请求数据 ===");
        Debug.WriteLine(FormatJson(jsonContent));
        Debug.WriteLine("================");

        HttpResponseMessage response = await client.PostAsync("http://120.26.136.102/mms_sm_req", httpContent);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        // 打印格式化后的响应数据
        Debug.WriteLine("=== 修改密码应答数据 ===");
        Debug.WriteLine(FormatJson(responseBody));
        Debug.WriteLine("================");

        return responseBody;
    }

    // 辅助方法：格式化JSON
    private static string FormatJson(string json)
    {
        try
        {
            var parsedJson = JToken.Parse(json);
            return parsedJson.ToString(Formatting.Indented);
        }
        catch
        {
            return json; // 如果格式化失败（比如不是标准JSON），直接返回原始
        }
    }

    // 通用响应解析方法
    public static (bool Success, string Message, string DebugMsg) ParseResponse(string responseJson)
    {
        try
        {
            var jsonResponse = JObject.Parse(responseJson);
            var code = jsonResponse["ANSWER"]?["HDR"]?["CODE"]?.ToString();
            var text = jsonResponse["ANSWER"]?["HDR"]?["TEXT"]?.ToString();
            var debugMsg = jsonResponse["ANSWER"]?["HDR"]?["DEBUG_MSG"]?.ToString();

            return (code == "0", text ?? "", debugMsg ?? "");
        }
        catch
        {
            return (false, "解析响应失败", "");
        }
    }

}
