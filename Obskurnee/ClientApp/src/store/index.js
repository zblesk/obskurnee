import { createStore } from 'vuex'
import context from './context'
import rounds from './rounds'
import discussions from './discussions'
import polls from './polls'

export default createStore({
  modules: {
    context,
    rounds,
    discussions,
    polls,
  }
})