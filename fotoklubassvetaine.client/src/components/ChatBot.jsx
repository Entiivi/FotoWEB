import React, { useState } from 'react';
import './css/ChatBot.css';

const ChatBot = () => {
    const [isOpen, setIsOpen] = useState(false);
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');

    const toggleChat = () => {
        setIsOpen(!isOpen);
    };

    const sendMessage = () => {
        if (input.trim() === '') return;
        setMessages([...messages, { text: input, sender: 'user' }]);
        setInput('');
    };

    return (
        <div className="chatbot-container">
            <button className="chatbot-button" onClick={toggleChat}>
                💬 Chat
            </button>
            {isOpen && (
                <div className="chatbot-window">
                    <div className="chatbot-header">
                        <span>Chat Assistant</span>
                        <button onClick={toggleChat}>✖</button>
                    </div>
                    <div className="chatbot-messages">
                        {messages.map((msg, index) => (
                            <div key={index} className={`chatbot-message ${msg.sender}`}>
                                {msg.text}
                            </div>
                        ))}
                    </div>
                    <div className="chatbot-input">
                        <input
                            type="text"
                            value={input}
                            onChange={(e) => setInput(e.target.value)}
                            placeholder="Type a message..."
                        />
                        <button onClick={sendMessage}>➤</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ChatBot;
