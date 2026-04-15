// using Scighost.PixivApi;
//
//
// namespace PixivApi.IntegrationTests;
//
// [TestClass]
// public sealed class FanboxClient
// {
//     [TestMethod]
//     public async Task GetSupportingPlansAsync()
//     {
//         var phpSess = "FANBOXSESSID=28410371_bwxEedhRbC9LIS9EgEkTna02QKH9FOQ2; ";
//         var cfbm = "__cf_bm=aU0IPmBM7oaPFtCVDcnxyullCJkQv3Lxza.c2mzEfK8-1755241670-1.0.1.1-mw518FkH4BggKF81LcG8Ukc_YCvn0HYUIeYAeA87s4p9CTGLgwVgjuOVhq0gjp9d__QVIpuRxEl.DZ.OssIEtPXf906mKey.JMRcPJu31.E;";
//         var cf_clearance = "cf_clearance=ePZ9qzEtZFgZIodR4Unc6ZgwoEs8_nprbIKeMTKIzFA-1755241670-1.2.1.1-TAhXGTqy.Zb8gYQ9clI3bZk.ux6hmnSkqC_rVphwqBUqocRZ9DI12hHi3I.9vEjLfkFAGmoZThr165ym6eMFmvKGV.bBuMDGVMUE7GsQ9nuXhPWjGrhbwTpDNlGSe.d2apSONuGWmaKEl0ePbBcU4ww1knEajtFmlN39AB3l3n5gcl1wAK0Zv0ky7H.6KuLM79dA.UHV3dclvOlNzkEefDtYPHNUWjpGwk6Gg307494;";
//
//         var cookie = phpSess + cfbm + cf_clearance;
//
//                      
//         
//         using var fanboxClient = new Scighost.PixivApi.FanboxClient(cookie,
//             enableCurlImpersonate: true);
//
//         var plans = await fanboxClient.GetSupportingPlansAsync();
//         var d = await fanboxClient.GetCreatorPostPaginationAsync("oyariashito");
//         var c = await fanboxClient.GetCreatorPostsFromPaginationAsync(d[0]);
//         foreach (var post in c)
//         {
//             _ = await fanboxClient.GetPostInfoAsync(post.Id);
//         }
//         Assert.IsTrue(plans.Length > 0);
//     }
//     
//     [TestMethod]
//     public async Task TestMethod1()
//     {
//         // using var client = new PixivClient(
//         //     cookie:
//         //     """__cf_bm=OQth1vvl.MZ2RjwvXbLC08_OB3k0oiRHS.0g64CsecA-1745867134-1.0.1.1-3wj0KdhZYMPnslGO4hAxThSGTx8PGau1JYutZUdgjQdOjJkgta5Ody419axMr.ZTZd2mbdSVQZwdGrsR1bND9iy0x3ytEtopNr_A9YScusi_D0N2EI1SnwYz5rb9dmTK;cf_clearance=7M.r0Me6gED8DXW9MWAHmVjq95mGctiUzhiDwMbr1CI-1745867486-1.2.1.1-RWl.zIUUs7CtLQ97UD9kTR5drxExEc0YpMatCEm8_r2S6ZnMLNhuivKyAp9hdWWudD2KVb3l_AAl4XMnxybxwgsP2GDKfxl1WD1Yu3cEj8VlyQC6k6iEiFsO3d0R4dY5egQkvEN6UhHLYYNb8MckGjEl5XHT_nvr3v8famNO8EDvzHthOEuN_zTm2Y03ZlatS7bB.4klBPnZSYWOu4s9dLDqV5T9tMDPacaZA5SkHISLgyqHpGo5TsjEKor_Dkrd93.ldLIOQT.naQhT4PMiCk4p143oaLcQeku7Nv6IEuXI3Xgie__1OaiFkKppHpw6BeJ71ZtR17T1pJZNbobiDtByrHirmGE7W1qIxBK7hXM;PHPSESSID=28410371_4pT1AQTBIlpnozpgTIkZu9J8pbE9dK3j;""");
//         //
//         // await client.GetTokenAsync();
//         // var allWorks = await client.GetUserAllWorksAsync(-1);
//         //
//         // Assert.AreEqual(437, allWorks.Illusts.Count);
//         
//         // using var client = new FanboxClient(
//         //     cookie: 
//         //     """__cf_bm=XD_355rOcmYDi5szICFNcwMYhvq6Aui.xCqXw6W7DPY-1746287352-1.0.1.1-3dgrsgmCFyLE5KRuloNI99k.cprZ.hWtzLn..8dsohc4j3.HR.gH20yhX4F4Ipg0BkieGsrUa.OBYR7BFxvr9dI63hc6S7QbYERLNV03kuo;cf_clearance=M1Ak.wjSKC318U5u9Xps1tizuaH4h.Z1wQjbxwj.dCw-1746286463-1.2.1.1-MGfv8jdIVn_jbi2RM5KACj8wMj9YLrdcjBOOQl_0upmxcP_vYXBBaY_6mCUIBENpvnPKneqXcv4ftzkaF6omefXywfV2MO5pbi1taXUKB32pIGKQdGxn5va36WBYnEfeF46h7g2BY6T1XlfWu76uh0X11HE.Tb6SEHqflLfHVZaUjeQfdTDymgPGrcv5D3uAWVNL2scULB.QrremS1rVX03p3h3KwfFrx8w0_p3tjpn1Tw7XwuwJTPErtOCe5uKiiCHp2ALVM3FAUFTP_.MVFdRg.TS69h2_xjbNWnQd8.Pi3w5XH6SNG4iUNwpVmcEdsQg_v7NI25AgqE6z2S5zGtpSrVrXdUPZtb2yUjUKzgk;FANBOXSESSID=28410371_RSHvkoIrfWcjZVfnmgfeufKEAonzMHo8""");
//         //
//         // var postInfo = await client.GetPostInfoAsync(9718001);
//         //
//         // await using var testFile = File.OpenWrite(@"D:\Test\13135\test.pdf");
//         //
//         // await client.DownloadFileAsync(
//         //         "https://downloads.fanbox.cc/files/post/9822421/VkcRyNVbLpstDJO7ikvzrVhc.pdf", testFile);
//
//         // var client = new PixivClientV2();
//         // // var getLoginUrl = client.GetCodeLoginUrl();
//         // // Console.WriteLine("\n\n\n\n\n\n\n\n");
//         // // Console.WriteLine(getLoginUrl);
//         // // Console.WriteLine("Enter the code after login in");
//         // // var code = Console.ReadLine();
//         // // await client.LoginAsync(code);
//         // await client.RefreshTokenAsync("tNND1EiCOfnS3_BqpQZgLxRYoIoEPpdHf5prh2nS0Q8");
//         // var search = await client.SearchIllustsAsync("ル・マラン(アズールレーン)", SearchOrderV2.DateDescending);
//         // Console.WriteLine(search);
//
//         var phpSess = "FANBOXSESSID=28410371_igT8JiHwARnZbcEL1zGQsNwT57d5qQBl; ";
//         var cfbm = "__cf_bm=3IwJ3cvmvocH2FmhmVxd9PQPIkOvr_h5cXs5JgzVQKk-1752217883-1.0.1.1-H92WPzjMP72E8B1u8qRE81yaGvKo6EvivEbcfRuQXO_qSLkHM9xWq9.j2EeKasqmO1BimrVq1qcgQyysjU3gX0YZQS2_V13_eY42eWfy0u0;";
//         var cf_clearance = "cf_clearance=0trR4Ia8WO4Mp.Gd_ETwnKOPbwkWZKVMQxVx53jVeFo-1752217884-1.2.1.1-U53OUe2DtvGVP1Fzq_lggAnsHgMgiyxmxL81G4OYVofHPm8F8y.SInvqEOhHMazi2qyHmb2w5UjGDJFZOYtWRbP85EtOlZN473Qt4qJ_IIt37k2Lju2Mt84jLQSkOWpoUkJJ84saaDJIpCQP5AuHGYAT18xxuqgC0qDnEwNzjPK8Xa601U5MydN9KPn4cLeL5IPPCkSXi7SCbs6QVI2cWIkdDyRuxI51agbmctH8AGY;";
//
//         var cookie = phpSess + cfbm + cf_clearance;
//
//         var curlPath = @"C:\Users\Ngand\Downloads\curl-impersonate-win\curl.exe";
//                      
//         
//         using var fanboxClient = new Scighost.PixivApi.FanboxClient(cookie, curlImpersonatePath: curlPath, 
//             enableCurlImpersonate: true);
//         using var secondClient = new Scighost.PixivApi.FanboxClient(cookie, enableCurlImpersonate: true);
//         var result = await fanboxClient.GetSupportingPlansAsync();
//         var pagination = await fanboxClient.GetCreatorPostPaginationAsync("oyariashito");
//         var paginatedResult = await fanboxClient.GetCreatorPostsFromPaginationAsync(
//             pagination[0]);
//         //var result = await fanboxClient.GetPostInfoAsync(9829595);
//     }
// }