using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace WinFormsDevExpressLoginDemo
{
    public partial class LoginForm : XtraForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // 这里只是演示，实际应安全处理密码和用户输入
            string token = await AuthenticateAsync(username, password);

            if (!string.IsNullOrEmpty(token))
            {
                XtraMessageBox.Show("登录成功！Token: " + token, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // TODO: 跳转到主界面
            }
            else
            {
                XtraMessageBox.Show("登录失败，请检查用户名或密码！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<string> AuthenticateAsync(string username, string password)
        {
            string apiUrl = "http://XXXX";
            string param = $"type=account";
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent("", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                var requestUri = $"{apiUrl}?{param}";
                try
                {
                    var response = await client.PostAsync(requestUri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        // 简单假设返回 JSON 里有 access_token 字段
                        // 可用 Newtonsoft.Json 或 System.Text.Json 解析
                        var token = ExtractToken(result);
                        return token;
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("网络异常: " + ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return null;
        }

        private string ExtractToken(string json)
        {
            // 简单提取token。建议使用Json库解析
            var key = "\"access_token\":\"";
            int idx = json.IndexOf(key);
            if (idx >= 0)
            {
                int start = idx + key.Length;
                int end = json.IndexOf("\"", start);
                if (end > start)
                {
                    return json.Substring(start, end - start);
                }
            }
            return null;
        }
    }
}
