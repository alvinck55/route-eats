import axios from 'axios'

const api = axios.create({ baseURL: '/api' })

// Attach JWT token to every request if present
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

export const getCategories = () =>
  api.get('/categories').then((r) => r.data)

export const getRouteSuggestions = (origin, destination, categories) =>
  api.post('/route/suggestions', { origin, destination, categories }).then((r) => r.data)

export const register = (email, password) =>
  api.post('/auth/register', { email, password }).then((r) => r.data)

export const login = (email, password) =>
  api.post('/auth/login', { email, password }).then((r) => r.data)

export const getSavedRoutes = () =>
  api.get('/user/saved-routes').then((r) => r.data)

export const saveRoute = (origin, destination, categories) =>
  api.post('/user/saved-routes', { origin, destination, categories }).then((r) => r.data)
