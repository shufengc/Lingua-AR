using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI
{
    public class AssistantChat : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        private float height;
        private OpenAIApi openai = new OpenAIApi("sk-proj-LRYIdKJRxgCG5rgNJ5TqT3BlbkFJ8HaYpexi5EdPC5tCIJhC", "org-PnIwYkAigOO5nukpqmqffRLy");

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "You're a language learning assistant. What you learn is that the users are now in a classroom with laptop, table, book, monitor, blackboard. Help users effectively learn foreign languages like Spanish or German or French step by step, offering clear instructions and explanation. Keep all your responses concise to mimic real-life conversations. Start by helping users with basic vocabulary or grammar questions.";

        private void Start()
        {
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        public async Task<string> SendReply(string input)
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = input
            };

            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + input;

            messages.Add(newMessage);
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4o-mini",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);
                AppendMessage(message);
                return message.Content;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
                return "";
            }

        }
    }
}
