export interface SearchResult {
  sourceName: string
  sourceLink: string
  text: string
}

export interface SearchState {
  results: SearchResult[]
  isLoading: boolean
  error: string | null
  hasSearched: boolean
  query: string
}

export interface AdvancedSearchParams {
  query: string
  source?: string
  maxResults?: number
  fromDate?: string
  toDate?: string
}
