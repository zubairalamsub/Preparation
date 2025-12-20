// This file is replaced during deployment
// For local development, uses localhost
// For production, this will be replaced with the actual API URL
(function(window) {
  window.__env = window.__env || {};
  window.__env.API_URL = 'https://preparation-fiab.onrender.com/api';
})(this);
