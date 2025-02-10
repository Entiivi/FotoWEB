import React from 'react';

function GitHubLoginButton() {
    const handleLogin = () => {
        window.location.href = 'https://localhost:7295/login/github';
    };

    return (
        <button onClick={handleLogin}>
            Login with GitHub
        </button>
    );
}

export default GitHubLoginButton;
