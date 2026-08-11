<template>
  <div>
    <div v-if="statsLoading" class="main reading">
      <h3 class="subsection-title">{{ $t('user.statsLoading') }}</h3>
    </div>
    <template v-else-if="userStats">
      <div class="main">
        <div class="reading">
            <span v-if="userStats.averageRating > 0">
                {{ $t('user.averageRating') }}: <strong>{{ userStats.averageRating.toFixed(2) }}</strong> <br />

                {{ $t('user.booksRead') }}: <strong>{{ userStats.booksReadCount }}</strong>
            </span>
          <span v-else class="recs-message">{{ $t('user.noRatingsYet') }}</span> <br />

            {{ $t('user.booksRecommended') }}: <strong>{{ userStats.booksRecommendedCount }}</strong> <br />
            {{ $t('user.booksUnread') }}: <strong>{{ userStats.unreadBooksCount }}</strong> <br />
        </div>
      </div>

      <div class="main">
        <div class="reading">
          <h3 class="subsection-title">{{ $t('user.unreadBooks') }}</h3>
          <div v-if="userStats.unreadBooks && userStats.unreadBooks.length > 0" class="books-grid">
            <book-preview v-bind:book="book" v-for="book in userStats.unreadBooks" v-bind:key="book.bookId">#{{ book.order }} </book-preview>
          </div>
          <p v-else class="recs-message">{{ $t('user.noUnreadBooks') }}</p>
        </div>
      </div>

      <div class="main">
        <div class="reading">
          <h3 class="subsection-title">{{ $t('user.votedButLost') }}</h3>
          <ul v-if="userStats.votedButLostSuggestions && userStats.votedButLostSuggestions.length > 0" class="reading-list">
            <li v-for="sug in userStats.votedButLostSuggestions" v-bind:key="'lost-' + sug.postId">
              <a v-if="sug.url" :href="sug.url">{{ sug.author }}: <strong>{{ sug.title }}</strong></a>
              <span v-else>{{ sug.author }}: <strong>{{ sug.title }}</strong></span>
            </li>
          </ul>
          <p v-else class="recs-message">{{ $t('user.noVotedButLost') }}</p>
        </div>
      </div>
    </template>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import BookPreview from './BookPreview.vue';
export default {
  name: 'UserStats',
  components: { BookPreview },
  props: {
    email: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      statsLoading: false
    }
  },
  computed: {
    ...mapGetters("stats", ["userDetailStatsById"]),
    userStats: function () { return this.email ? this.userDetailStatsById(this.email) : null; },
  },
  watch: {
    email() {
      this.loadUserStats();
    }
  },
  methods: {
    ...mapActions("stats", ["fetchUserDetailStats"]),
    loadUserStats()
    {
      if (this.statsLoading || !this.email)
      {
        return;
      }
      // Already cached for this user - nothing to do.
      if (this.userDetailStatsById(this.email))
      {
        return;
      }
      this.statsLoading = true;
      this.fetchUserDetailStats(this.email)
        .catch(this.$handleApiError)
        .finally(() => this.statsLoading = false);
    },
  },
  mounted() {
    this.loadUserStats();
  }
}
</script>

<style scoped>
  .books-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
    gap: var(--spacer);
    padding: var(--spacer);
  }

  .stats-summary {
    padding: var(--spacer);
  }

  .reading {

  }
</style>
