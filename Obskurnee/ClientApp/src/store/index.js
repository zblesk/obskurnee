import { createStore } from 'vuex'
import context from './context'
import rounds from './rounds'

export default createStore({
  modules: {
    context,
    rounds
  }
})