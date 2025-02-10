import React from 'react';

function GoogleLoginButton() {
    const handleLogin = () => {
        window.location.href = "https://localhost:5173/signin-oidc"; // Replace with your backend URL
    };

    return (
        <button onClick={handleLogin}>
            Login with Google
        </button>
    );
}

export default GoogleLoginButton;
