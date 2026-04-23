import { test, expect, type Page } from '@playwright/test'

// Mock search results
const mockResults = [
  {
    sourceName: 'Bank Transactions',
    sourceLink: 'https://example.com/transactions',
    text: 'Return transaction processed on 2024-01-15 for account ending 4521.',
  },
  {
    sourceName: 'Account Statements',
    sourceLink: 'https://example.com/statements',
    text: 'Monthly return statement showing balance of $12,450.00.',
  },
]

const BASIC_API_PATTERN = '**/basic/search*'
const ADVANCED_API_PATTERN = '**/advanced/search*'

// ─── Basic Search Page Object ────────────────────────────────────────────────
class SearchPage {
  constructor(private page: Page) {}

  async goto() {
    await this.page.goto('/')
  }

  get searchInput() {
    return this.page.getByTestId('search-input')
  }

  get searchButton() {
    return this.page.getByTestId('search-button')
  }

  get validationError() {
    return this.page.getByTestId('validation-error')
  }

  get resultsList() {
    return this.page.getByTestId('results-list')
  }

  get resultCards() {
    return this.page.getByTestId('result-card')
  }

  get loadingState() {
    return this.page.getByTestId('loading-state')
  }

  get errorState() {
    return this.page.getByTestId('error-state')
  }

  get emptyState() {
    return this.page.getByTestId('empty-state')
  }

  async mockApiSuccess(results = mockResults) {
    await this.page.route(BASIC_API_PATTERN, (route) => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(results),
      })
    })
  }

  async mockApiEmpty() {
    await this.page.route(BASIC_API_PATTERN, (route) => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify([]),
      })
    })
  }

  async mockApiError() {
    await this.page.route(BASIC_API_PATTERN, (route) => {
      route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify({ error: 'Internal Server Error' }),
      })
    })
  }

  async search(query: string) {
    await this.searchInput.fill(query)
    await this.searchButton.click()
  }
}

// ─── Advanced Search Page Object ─────────────────────────────────────────────
class AdvancedSearchPage {
  constructor(private page: Page) {}

  async goto() {
    await this.page.goto('/advanced')
  }

  get queryInput() {
    return this.page.getByTestId('advanced-query-input')
  }

  get searchButton() {
    return this.page.getByTestId('advanced-search-button')
  }

  get validationError() {
    return this.page.getByTestId('advanced-validation-error')
  }

  get resultsList() {
    return this.page.getByTestId('results-list')
  }

  get resultCards() {
    return this.page.getByTestId('result-card')
  }

  get emptyState() {
    return this.page.getByTestId('empty-state')
  }

  get errorState() {
    return this.page.getByTestId('error-state')
  }

  get clearButton() {
    return this.page.getByTestId('advanced-clear-button')
  }

  async mockApiSuccess(results = mockResults) {
    await this.page.route(ADVANCED_API_PATTERN, (route) => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(results),
      })
    })
  }

  async mockApiEmpty() {
    await this.page.route(ADVANCED_API_PATTERN, (route) => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify([]),
      })
    })
  }

  async mockApiError() {
    await this.page.route(ADVANCED_API_PATTERN, (route) => {
      route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify({ error: 'Internal Server Error' }),
      })
    })
  }

  async search(query: string) {
    await this.queryInput.fill(query)
    await this.searchButton.click()
  }
}

// ─── Basic Search Tests ───────────────────────────────────────────────────────
test.describe('Enterprise Search App', () => {
  test('loads the search page with input and button visible', async ({ page }) => {
    const searchPage = new SearchPage(page)
    await searchPage.goto()

    await expect(searchPage.searchInput).toBeVisible()
    await expect(searchPage.searchButton).toBeVisible()
    await expect(searchPage.searchInput).toBeEnabled()
    await expect(page).toHaveTitle(/ASE/)
  })

  test.describe('Validation', () => {
    test('shows error when submitting empty query', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.goto()

      await searchPage.searchButton.click()

      await expect(searchPage.validationError).toBeVisible()
      await expect(searchPage.validationError).toContainText('Please enter a search query')
    })

    test('shows error when query is only whitespace', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.goto()

      await searchPage.searchInput.fill('   ')
      await searchPage.searchButton.click()

      await expect(searchPage.validationError).toBeVisible()
      await expect(searchPage.validationError).toContainText('Please enter a search query')
    })

    test('shows error when query is a single character', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.goto()

      await searchPage.searchInput.fill('a')
      await searchPage.searchButton.click()

      await expect(searchPage.validationError).toBeVisible()
      await expect(searchPage.validationError).toContainText('at least 2 characters')
    })

    test('clears validation error when user starts typing', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.goto()

      await searchPage.searchButton.click()
      await expect(searchPage.validationError).toBeVisible()

      await searchPage.searchInput.fill('b')
      await expect(searchPage.validationError).not.toBeVisible()
    })
  })

  test.describe('Search results', () => {
    test('displays results after a successful search', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.mockApiSuccess()
      await searchPage.goto()

      await searchPage.search('return')

      await expect(searchPage.resultsList).toBeVisible()
      await expect(searchPage.resultCards).toHaveCount(2)
    })

    test('shows correct source name and text in result cards', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.mockApiSuccess()
      await searchPage.goto()

      await searchPage.search('return')

      const firstCard = searchPage.resultCards.first()
      await expect(firstCard.getByTestId('result-source')).toContainText('Bank Transactions')
      await expect(firstCard.getByTestId('result-text')).toContainText('Return transaction')
    })

    test('shows result link when sourceLink is provided', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.mockApiSuccess()
      await searchPage.goto()

      await searchPage.search('return')

      const firstCard = searchPage.resultCards.first()
      await expect(firstCard.getByTestId('result-link')).toBeVisible()
      await expect(firstCard.getByTestId('result-link')).toHaveAttribute('href', 'https://example.com/transactions')
    })

    test('shows empty state when search returns no results', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.mockApiEmpty()
      await searchPage.goto()

      await searchPage.search('xyzzy')

      await expect(searchPage.emptyState).toBeVisible()
      await expect(searchPage.emptyState).toContainText('No results found')
    })

    test('shows error state when API returns an error', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.mockApiError()
      await searchPage.goto()

      await searchPage.search('return')

      await expect(searchPage.errorState).toBeVisible()
    })

    test('search button is disabled while loading', async ({ page }) => {
      const searchPage = new SearchPage(page)
      // Slow mock so we can catch the loading state
      await page.route(BASIC_API_PATTERN, async (route) => {
        await new Promise((resolve) => setTimeout(resolve, 500))
        route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockResults),
        })
      })
      await searchPage.goto()

      await searchPage.searchInput.fill('return')
      await searchPage.searchButton.click()

      await expect(searchPage.searchButton).toBeDisabled()
      // Wait for results to appear
      await expect(searchPage.resultsList).toBeVisible({ timeout: 5000 })
    })

    test('pressing Enter in search input triggers search', async ({ page }) => {
      const searchPage = new SearchPage(page)
      await searchPage.mockApiSuccess()
      await searchPage.goto()

      await searchPage.searchInput.fill('return')
      await searchPage.searchInput.press('Enter')

      await expect(searchPage.resultsList).toBeVisible()
    })
  })
})

// ─── Advanced Search Tests ────────────────────────────────────────────────────
test.describe('Advanced Search', () => {
  test('loads the advanced search page with only query input and search button', async ({ page }) => {
    const advPage = new AdvancedSearchPage(page)
    await advPage.goto()

    await expect(advPage.queryInput).toBeVisible()
    await expect(advPage.searchButton).toBeVisible()
    // Ensure filter fields that were removed are NOT present
    await expect(page.locator('#adv-source')).not.toBeVisible()
    await expect(page.locator('#adv-max')).not.toBeVisible()
    await expect(page.locator('#adv-from')).not.toBeVisible()
    await expect(page.locator('#adv-to')).not.toBeVisible()
  })

  test('calls /advanced/search endpoint with query parameter', async ({ page }) => {
    let capturedUrl = ''
    await page.route(ADVANCED_API_PATTERN, (route) => {
      capturedUrl = route.request().url()
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(mockResults),
      })
    })

    const advPage = new AdvancedSearchPage(page)
    await advPage.goto()
    await advPage.search('return')

    expect(capturedUrl).toContain('/advanced/search')
    expect(capturedUrl).toContain('query=return')
  })

  test.describe('Validation', () => {
    test('shows error when submitting empty query', async ({ page }) => {
      const advPage = new AdvancedSearchPage(page)
      await advPage.goto()

      await advPage.searchButton.click()

      await expect(advPage.validationError).toBeVisible()
      await expect(advPage.validationError).toContainText('Please enter a search query')
    })

    test('shows error when query is a single character', async ({ page }) => {
      const advPage = new AdvancedSearchPage(page)
      await advPage.goto()

      await advPage.queryInput.fill('x')
      await advPage.searchButton.click()

      await expect(advPage.validationError).toBeVisible()
      await expect(advPage.validationError).toContainText('at least 2 characters')
    })
  })

  test.describe('Results', () => {
    test('displays results after a successful advanced search', async ({ page }) => {
      const advPage = new AdvancedSearchPage(page)
      await advPage.mockApiSuccess()
      await advPage.goto()

      await advPage.search('return')

      await expect(advPage.resultsList).toBeVisible()
      await expect(advPage.resultCards).toHaveCount(2)
    })

    test('shows empty state when advanced search returns no results', async ({ page }) => {
      const advPage = new AdvancedSearchPage(page)
      await advPage.mockApiEmpty()
      await advPage.goto()

      await advPage.search('xyzzy')

      await expect(advPage.emptyState).toBeVisible()
    })

    test('shows error state when API returns an error', async ({ page }) => {
      const advPage = new AdvancedSearchPage(page)
      await advPage.mockApiError()
      await advPage.goto()

      await advPage.search('return')

      await expect(advPage.errorState).toBeVisible()
    })
  })

  test.describe('Highlighting', () => {
    test('highlights matching query text in result cards', async ({ page }) => {
      const advPage = new AdvancedSearchPage(page)
      await advPage.mockApiSuccess()
      await advPage.goto()

      await advPage.search('return')

      // At least one highlight mark should be present
      const highlights = page.locator('[data-testid="highlight"]')
      await expect(highlights.first()).toBeVisible()
    })

    test('highlight marks contain the searched term (case-insensitive)', async ({ page }) => {
      const advPage = new AdvancedSearchPage(page)
      await advPage.mockApiSuccess()
      await advPage.goto()

      await advPage.search('return')

      const firstHighlight = page.locator('[data-testid="highlight"]').first()
      const text = await firstHighlight.textContent()
      expect(text?.toLowerCase()).toBe('return')
    })

    test('clears highlights after reset', async ({ page }) => {
      const advPage = new AdvancedSearchPage(page)
      await advPage.mockApiSuccess()
      await advPage.goto()

      await advPage.search('return')
      await expect(page.locator('[data-testid="highlight"]').first()).toBeVisible()

      await advPage.clearButton.click()

      await expect(advPage.resultsList).not.toBeVisible()
    })
  })
})
