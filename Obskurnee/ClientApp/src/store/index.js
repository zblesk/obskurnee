import { createStore } from 'vuex'
import context from './context'
import rounds from './rounds'
import discussions from './discussions'

export default createStore({
  modules: {
    context,
    rounds,
    discussions
  }
})