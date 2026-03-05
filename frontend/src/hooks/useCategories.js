import { useState, useEffect } from 'react'
import { getCategories } from '../services/api'

export function useCategories() {
  const [categories, setCategories] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    getCategories()
      .then(setCategories)
      .catch(setError)
      .finally(() => setLoading(false))
  }, [])

  return { categories, loading, error }
}
