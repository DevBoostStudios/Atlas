using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Weather : ModuleBase<SocketCommandContext>
    {
        private IConfiguration _config;

        [Command("weather")]
        [Summary("Returns the current Weather details for the specified location.")]
        public async Task WeatherCmd([Remainder] string location)
        {
            using (Context.Channel.EnterTypingState())
            {
                using (var client = new HttpClient())
                {
                    _config = BuildConfig();

                    var json = await client.GetStringAsync("http://api.openweathermap.org/data/2.5/weather?q=" + location + "&units=imperial&appid=" + _config["openWeatherMapKey"]);
                    dynamic parse = JsonConvert.DeserializeObject(json);

                    var forecastJSON = await client.GetStringAsync("http://api.openweathermap.org/data/2.5/forecast?q=" + location + "&units=imperial&appid=" + _config["openWeatherMapKey"]);
                    dynamic forecastParse = JsonConvert.DeserializeObject(forecastJSON);

                    string loc = parse.name;
                    string description = parse.weather[0].description;
                    double temperature = parse.main.temp;
                    double temperatureC = 5.0 / 9.0 * (temperature - 32);
                    double tempHigh = parse.main.temp_max;
                    double tempHighC = 5.0 / 9.0 * (tempHigh - 32);
                    double tempLow = parse.main.temp_min;
                    double tempLowC = 5.0 / 9.0 * (tempLow - 32);
                    string humidity = parse.main.humidity;
                    string windSpeed = parse.wind.speed;
                    string pressure = parse.main.pressure;

                    string day1Init = forecastParse.list[0].dt_txt;
                    string day1 = day1Init.Split(' ')[0]; // To Do: Get name of Day of Week
                    double day1TempHigh = forecastParse.list[0].main.temp_max;
                    double day1TempLow = forecastParse.list[0].main.temp_min;
                    string day2Init = forecastParse.list[1].dt_txt;
                    string day2 = day2Init.Split(' ')[0]; // To Do: Get name of Day of Week
                    double day2TempHigh = forecastParse.list[1].main.temp_max;
                    double day2TempLow = forecastParse.list[1].main.temp_min;
                    string day3Init = forecastParse.list[2].dt_txt;
                    string day3 = day3Init.Split(' ')[0]; // To Do: Get name of Day of Week
                    double day3TempHigh = forecastParse.list[2].main.temp_max;
                    double day3TempLow = forecastParse.list[2].main.temp_min;

                    var builder = new EmbedBuilder()
                        .WithColor(new Color(5025616))
                        .WithAuthor(author =>
                        {
                            author
                            .WithName("Weather - " + loc)
                            .WithIconUrl("http://i.imgur.com/qKyhmG9.png");
                        })
                        .WithDescription(description.Substring(0, 1).ToUpper() + description.Substring(1))
                        .AddInlineField("Temperature", Math.Round(temperature, 0) + " °F (" + Math.Round(temperatureC, 0) + " °C)")
                        .AddInlineField("High", Math.Round(tempHigh, 0) + " °F (" + Math.Round(tempHighC, 0) + " °C)")
                        .AddInlineField("Low", Math.Round(tempLow, 0) + " °F (" + Math.Round(tempLowC, 0) + " °C)")
                        .AddInlineField("Humidity", humidity + "%")
                        .AddInlineField("Wind Speed", windSpeed + " mph")
                        .AddInlineField("Pressure", pressure + " Pa")
                        .AddField("​", "__**3 Day Forecast**__")
                        .AddInlineField(day1, Math.Round(day1TempHigh, 0) + " °F / " + Math.Round(day1TempLow, 0) + " °F")
                        .AddInlineField(day2, Math.Round(day2TempHigh, 0) + " °F / " + Math.Round(day2TempLow, 0) + " °F")
                        .AddInlineField(day3, Math.Round(day3TempHigh, 0) + " °F / " + Math.Round(day3TempLow, 0) + " °F")
                        .WithFooter(footer =>
                        {
                            footer
                            .WithText(Context.User.ToString() + " | " + DateTime.Now.ToString())
                            .WithIconUrl(Context.User.GetAvatarUrl());
                        });
                    var embed = builder.Build();
                    await ReplyAsync("", false, embed)
                        .ConfigureAwait(false);
                }
            }
        }

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }
    }
}
