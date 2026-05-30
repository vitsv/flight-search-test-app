import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000',
})

export function fetchOrigins() {
  return api.get('/api/airports')
}

export function fetchDestinations(originCode) {
  return api.get(`/api/airports/destinations/${encodeURIComponent(originCode)}`)
}
