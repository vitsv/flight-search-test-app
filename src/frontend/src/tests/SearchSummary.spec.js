import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import SearchSummary from '../components/SearchSummary.vue'

const baseResult = {
  tripType: 'one-way',
  origin: { code: 'LHR', label: 'LHR — London Heathrow' },
  destination: { code: 'CDG', label: 'CDG — Paris Charles de Gaulle' },
  departureDate: null,
  returnDate: null,
  passengers: 1,
}

function mountWith(overrides = {}) {
  return mount(SearchSummary, {
    props: { result: { ...baseResult, ...overrides } },
  })
}

describe('SearchSummary', () => {
  it('displays "One-way" for one-way trip type', () => {
    const wrapper = mountWith({ tripType: 'one-way' })
    expect(wrapper.text()).toContain('One-way')
  })

  it('displays "Return" for return trip type', () => {
    const wrapper = mountWith({ tripType: 'return' })
    expect(wrapper.text()).toContain('Return')
  })

  it('displays origin and destination labels', () => {
    const wrapper = mountWith()
    expect(wrapper.text()).toContain('LHR — London Heathrow')
    expect(wrapper.text()).toContain('CDG — Paris Charles de Gaulle')
  })

  it('shows formatted departure date when provided', () => {
    const wrapper = mountWith({ departureDate: new Date(2026, 5, 15) }) // 15 Jun 2026
    expect(wrapper.text()).toContain('Jun')
    expect(wrapper.text()).toContain('2026')
  })

  it('shows passenger count', () => {
    const wrapper = mountWith({ passengers: 3 })
    expect(wrapper.text()).toContain('3')
    expect(wrapper.text()).toContain('passengers')
  })

  it('uses singular "passenger" for count of 1', () => {
    const wrapper = mountWith({ passengers: 1 })
    expect(wrapper.text()).toContain('1')
    expect(wrapper.text()).toContain('passenger')
    expect(wrapper.text()).not.toContain('passengers')
  })
})
