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

    /* theme*/
    const toggleTheme = () => {
        setTheme((prevTheme) => (prevTheme === 'light' ? 'dark' : 'light'));
    };

    // Mygtukas rodomas tik jei vartotojas yra "/main" puslapyje
    if (location.pathname !== '/main') return null;

    /* theme*/
    return (
        <button onClick={toggleTheme} className="theme-toggle">
            {theme === 'light' ? 'Dark Mode' : 'Light Mode'}
        </button>
    );
}
/* Chatbot*/
function ChatBotWrapper() {
    const location = useLocation();
    if (location.pathname !== '/main') return null;
    return <ChatBot />;
}

function App() {
    return (
        <Router>
            <div className="app-container">
                <ThemeToggle />
                <ChatBotWrapper /> {/* Chatbotas rodomas tik "/main" puslapyje */}
                <Routes>
                    <Route path="/login" element={<LoginForm />} />
                    <Route path="/main" element={<MainForm />} />
                    <Route path="/photo-preview" element={<PhotoPreview />} />
                    <Route path="/upload-photo" element={<UploadPhoto />} />
                    <Route path="/personal-info" element={<PersonalInfo />} />
                    <Route path="/login-info" element={<LoginInformation />} />
                    <Route path="/create-account" element={<CreateAccount />} />
                    <Route path="/photo-details" element={<PhotoDetails />} />
                    <Route path="*" element={<Navigate to="/login" />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
