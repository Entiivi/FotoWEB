import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate, useLocation } from 'react-router-dom';
import LoginForm from './components/LoginForm.jsx';
import MainForm from './components/MainForm.jsx';
import PhotoPreview from './components/PhotoPreview.jsx';
import UploadPhoto from './components/UploadPhoto.jsx';
import PersonalInfo from './components/PersonalInfo.jsx';
import CreateAccount from './components/CreateAccount.jsx';
import LoginInformation from './components/LoginInfoForm.jsx';
import PhotoDetails from './components/PhotoDetails.jsx';
import ChatBot from './components/ChatBot.jsx';

import './App.css';

function ThemeToggle() {
    const [theme, setTheme] = useState(localStorage.getItem('theme') || 'light');
    const location = useLocation();

    useEffect(() => {
        document.body.className = theme;
        localStorage.setItem('theme', theme);
    }, [theme]);

    // Only show the toggle on the '/main' page
    if (location.pathname !== '/main') return null;

    const toggleTheme = () => {
        setTheme((prevTheme) => (prevTheme === 'light' ? 'dark' : 'light'));
    };

    return (
        <button onClick={toggleTheme} className="theme-toggle">
            {theme === 'light' ? 'Dark Mode' : 'Light Mode'}
        </button>
    );
}

function ChatBotWrapper() {
    const location = useLocation();
    // Only show the ChatBot on the '/main' page
    if (location.pathname !== '/main') return null;
    return <ChatBot />;
}

function App() {
    // Create a state to store logged-in user info
    const [user, setUser] = useState(null);

    // Update the login handler to store the username and user id (narysID)
    const handleLogin = (username, narysID) => {
        console.log('User logged in:', username);
        setUser({ username, narysID });
    };

    return (
        <Router>
            <div className="app-container">
                <ThemeToggle />
                <ChatBotWrapper /> {/* Chatbot is only shown on '/main' */}
                <Routes>
                    {/* Pass the required onLogin prop to LoginForm */}
                    <Route path="/login" element={<LoginForm onLogin={handleLogin} />} />
                    <Route path="/main" element={<MainForm />} />
                    <Route path="/photo-preview" element={<PhotoPreview />} />
                    <Route path="/upload-photo" element={<UploadPhoto username = { user? user.username : ''} />}/>
                    {/* Pass the username as a prop to PersonalInfo */}
                    <Route path="/personal-info" element={<PersonalInfo username={user ? user.username : ''} />} />
                    <Route
                        path="/login-info"
                        element={
                            <LoginInformation
                                userId={user ? user.narysID : ''}
                                username={user ? user.username : ''}
                            />
                        }
                    />
                    <Route path="/create-account" element={<CreateAccount />} />
                    <Route path="/photo-details" element={<PhotoDetails />} />
                    <Route path="*" element={<Navigate to="/login" />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
