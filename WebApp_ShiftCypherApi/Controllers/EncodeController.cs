using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp_ShiftCypherApi.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Text;
using NLog;

namespace WebApp_ShiftCypherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncodeController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public object Encode([FromBody] Encode content)
        {
           
            if ((content.Message == null) || (content.Shift < 0 )) { // make sure that we have a valid Encode object
                var EncodedMessage = string.Empty; //empty string
                var jsonenErrorMessage = new // Create my object so that I can serialize EncodedMessage Info 
                {
                    EncodedMessage
                };
                var payload = JsonConvert.SerializeObject(jsonenErrorMessage); // convert from jsonencipheredMessage C# object to json string
                Response.StatusCode = 500; // Server Error 
                return payload;
            }
            else
            {
                string EncodedMessage = Encipher(content.Message, content.Shift);
                var jsonencipheredMessage = new // Create my object so that I can serialize EncodedMessage Info 
                {
                    EncodedMessage
                };
                var payload = JsonConvert.SerializeObject(jsonencipheredMessage); // convert from jsonencipheredMessage C# object to json string
                string path = System.Reflection.Assembly.GetEntryAssembly().Location + "ShiftCipher.txt"; // save file to the directory of this app
                WriteCipherToFile(path, payload);
                Response.StatusCode = 200; // OK 
                return payload;
            }

           
        }

        /// <summary>
        /// This method writes the enciphered string to the file path provided in the path parameter   
        /// </summary>
        public static void  WriteCipherToFile(string path, string cipher) 
        {           
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.Create(Path.GetDirectoryName(path)).Dispose();

                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.WriteLine(cipher);
                    }

                }
                else  
                {
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.WriteLine(cipher);
                    }
                }

                logger.Log(LogLevel.Info, "Writing cipher to file success" + path);                
            } 
            catch (Exception ex) 
            {
                logger.Log(LogLevel.Error, "Error writing cipher to file " + ex.ToString());
            }              
        
        }

        /// <summary>
        /// This method loops over each characted in the un-enciphered string and encrypts this caracter using the cipher method
        /// </summary>
        public static string Encipher(string input, int key)
        {
            logger.Log(LogLevel.Info, "In the Encipher() method");
            string output = string.Empty;

            try
            {
                foreach (char ch in input)
                    output += cipher(ch, key);
            }
            catch (Exception ex) {
                output = "";
                logger.Log(LogLevel.Error, "Unable to encipher " + ex.ToString());
            }
            return output;
        }

        /// <summary>
        /// This method accepts a cgaracter to encrypt and returns the encrypted character
        /// </summary>
        public static char cipher(char ch, int key)
        {
            if (!char.IsLetter(ch))
            {

                return ch;
            }

            char d = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((((ch + key) - d) % 26) + d);

        }
    }
}
