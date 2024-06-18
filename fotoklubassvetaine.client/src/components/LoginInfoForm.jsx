import React from 'react';
import PropTypes from 'prop-types';
import './css/LoginInfoForm.css';

const LoginInformation = ({ username, password }) => {
    return (
        <div className="login-info-container">
            <h2>Login Information</h2>
            <p>Username: {username}</p>
            <p>Password: {password}</p>
        </div>
    );
};

LoginInformation.propTypes = {
    username: PropTypes.string.isRequired,
    password: PropTypes.string.isRequired,
};

export default LoginInformation;
