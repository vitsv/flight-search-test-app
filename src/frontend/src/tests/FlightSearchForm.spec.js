import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount, flushPromises } from '@vue/test-utils'
import FlightSearchForm from '../components/FlightSearchForm.vue'

vi.mock('../api.js', () => ({
  fetchOrigins: vi.fn(),
  fetchDestinations: vi.fn(),
}))

import { fetchOrigins, fetchDestinations } from '../api.js'

const mockOrigins = [
  { code: 'LHR', name: 'London Heathrow' },
  { code: 'CDG', name: 'Paris Charles de Gaulle' },
]

const mockDestinations = [
  { code: 'JFK', name: 'New York JFK' },
  { code: 'DXB', name: 'Dubai International' },
]

describe('FlightSearchForm', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    fetchOrigins.mockResolvedValue({ data: mockOrigins })
    fetchDestinations.mockResolvedValue({ data: mockDestinations })
  })

  it('fetches origins on mount', async () => {
    mount(FlightSearchForm)
    await flushPromises()

    expect(fetchOrigins).toHaveBeenCalledOnce()
  })

  it('populates origins list after successful fetch', async () => {
    const wrapper = mount(FlightSearchForm)
    await flushPromises()

    expect(wrapper.vm.origins).toHaveLength(2)
    expect(wrapper.vm.origins[0].code).toBe('LHR')
  })

  it('fetches destinations when origin changes', async () => {
    const wrapper = mount(FlightSearchForm)
    await flushPromises()

    await wrapper.vm.onOriginChange('LHR')
    await flushPromises()

    expect(fetchDestinations).toHaveBeenCalledWith('LHR')
    expect(wrapper.vm.destinations).toHaveLength(2)
  })

  it('clears destination and search result when origin changes', async () => {
    const wrapper = mount(FlightSearchForm)
    await flushPromises()

    // Set up an initial state
    wrapper.vm.destination = 'JFK'
    wrapper.vm.searchResult = { tripType: 'one-way' }

    await wrapper.vm.onOriginChange('CDG')
    await flushPromises()

    expect(wrapper.vm.destination).toBeNull()
    expect(wrapper.vm.searchResult).toBeNull()
  })

  it('sets searchResult with correct values on search', async () => {
    const wrapper = mount(FlightSearchForm)
    await flushPromises()

    await wrapper.vm.onOriginChange('LHR')
    await flushPromises()

    wrapper.vm.origin = 'LHR'
    wrapper.vm.destination = 'JFK'
    wrapper.vm.passengers = 2

    wrapper.vm.onSearch()

    expect(wrapper.vm.searchResult).toMatchObject({
      tripType: 'one-way',
      passengers: 2,
    })
    expect(wrapper.vm.searchResult.origin.code).toBe('LHR')
    expect(wrapper.vm.searchResult.destination.code).toBe('JFK')
  })
})
