import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { useNavigate } from 'react-router-dom';
import './css/LoginForm.css';

const LoginForm = ({ onLogin }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch('https://localhost:7295/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username, password }),
            });

            if (response.ok) {
                const data = await response.json();
                console.log('Login successful:', data);
                onLogin(username, password); // Pass username and password to the handler
                navigate('/main'); // Redirect to MainForm page
            } else {
                console.error('Login failed');
            }
        } catch (error) {
            console.error('Error during login:', error);
        }
    };

    const handleGoogleLogin = () => {
        // Redirect to your backend Google login endpoint
        window.location.href = 'https://localhost:7295/login/google'; // Update with your backend's Google login URL
    };

    const handleGitHubLogin = () => {
        // Redirect to your backend GitHub login endpoint
        window.location.href = 'https://localhost:7295/login/github'; // Update with your backend's GitHub login URL
    };

    return (
        <div className="login-form-wrapper">
            <div className="login-form-container">
                <img src="Logo.png" alt="Logo" />
                <form onSubmit={handleLogin}>
                    <div>
                        <label>Username</label>
                        <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
                    </div>
                    <div>
                        <label>Password</label>
                        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                    </div>
                    <div className="button-container">
                        <button type="submit">Login</button>
                        <button type="button" className="create-account" onClick={() => navigate('/create-account')}>
                            Create an account
                        </button>
                    </div>
                </form>
                <div className="google-login-container">
                    <button onClick={handleGoogleLogin} className="google-login-button">
                        Login with Google
                    </button>
                </div>
                <button onClick={handleGitHubLogin} className="github-login-button">
                    Login with GitHub
                </button>
            </div>
        </div>
    );
};

LoginForm.propTypes = {
    onLogin: PropTypes.func.isRequired,
};

export default LoginForm;
