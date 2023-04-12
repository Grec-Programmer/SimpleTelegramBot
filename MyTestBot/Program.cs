using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace MyTestBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var clientBor = new TelegramBotClient("6167273630:AAFaajU_fsU5dDounWLREFYjWG7hfINb5iE");
            clientBor.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        async private static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if(message.Text != null)
            {
                Console.WriteLine($"{message.Chat.FirstName}   |   {message.Text}");
                if (message.Text.ToLower().Contains("здорова"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,"Здоровей Видали!");
                    return;
                }
            }
            if(message.Photo != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id,"Фото неплохое. Но мне нужен документ!");
                return;
            }
            if (message.Document != null)
            {
                string photo = $@"C:\Users\allap\OneDrive\Рабочий стол\Мой Пёс.jpg";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Сейчас всё устрою! Сделаем в лучшем виде...)");

                var fileId = update.Message.Document.FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;

                string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\{message.Document.FileName}";
                using (FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath))
                await botClient.DownloadFileAsync(filePath,fileStream);

                using (Stream stream = System.IO.File.OpenRead(photo))
                    await botClient.SendDocumentAsync(message.Chat.Id, new InputOnlineFile(stream,message.Document.FileName.Replace(".jpg"," (edited).jpg")));

                    return;
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
