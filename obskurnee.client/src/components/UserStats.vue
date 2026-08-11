<template>
  <div>
    <div v-if="statsLoading" class="main reading">
      <h3 class="subsection-title">{{ $t('user.statsLoading') }}</h3>
    </div>
    <template v-else-if="userStats">
        <div class="main">
            <div>
                <span v-if="userStats.averageRating > 0">
                    {{ $t('user.averageRating') }}:
                    <star-rating :rating="userStats.averageRating" /> <br />

                    {{ $t('user.booksRead') }}: <strong>{{ userStats.booksReadCount }}</strong>
                </span>
                <span v-else class="recs-message">{{ $t('user.noRatingsYet') }}</span> <br />

                {{ $t('user.booksRecommended') }}: <strong>{{ userStats.booksRecommendedCount }}</strong> <br />
                {{ $t('user.booksUnread') }}: <strong>{{ userStats.unreadBooksCount }}</strong> <br />
            </div>
        </div>

        <h2 class="page-subtitle page-subtitle--flex u-mt-sp">
            <span>{{ $t('user.unreadBooks') }}</span>
            <img src="../assets/download.svg" alt="toggle unread books"
                 v-if="userStats.unreadBooks && userStats.unreadBooks.length > 0" @click="showUnread = !showUnread"
                 class="toggler" :class="{ 'toggler--hidden': !showUnread }">
        </h2>
        <div v-if="userStats.unreadBooks && userStats.unreadBooks.length > 0">
            <div v-if="showUnread">
                <div class="books-grid">
                    <book-preview v-bind:book="book" v-for="book in userStats.unreadBooks" v-bind:key="book.bookId">#{{ book.order }} </book-preview>
                </div>
            </div>
        </div>
        <p v-else class="recs-message">{{ $t('user.noUnreadBooks') }}</p>

        <h2 class="page-subtitle page-subtitle--flex u-mt-sp">
            <span>{{ $t('user.votedButLost') }}</span>
            <img src="../assets/download.svg" alt="toggle voted but lost"
                 v-if="userStats.votedButLostSuggestions && userStats.votedButLostSuggestions.length > 0" @click="showVoted = !showVoted"
                 class="toggler" :class="{ 'toggler--hidden': !showVoted }">
        </h2>
        <div class="books-grid">
            <template v-for="bookPost in userStats.votedButLostSuggestions"
                      v-bind:key="bookPost.postId"
                      v-bind:result="bookPost">
                <book-post-preview :post="bookPost" />
            </template>
        </div>
        <div v-if="userStats.votedButLostSuggestions && userStats.votedButLostSuggestions.length > 0">
            <div v-if="showVoted">
            </div>
        </div>
        <p v-else class="recs-message">{{ $t('user.noVotedButLost') }}</p>
    </template>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import BookPreview from './BookPreview.vue';
import StarRating from './StarRating.vue';
import BookPostPreview from "./BookPostPreview.vue";
export default {
  name: 'UserStats',
  components: { BookPreview, StarRating, BookPostPreview },
  props: {
    email: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      statsLoading: false,
      showUnread: true,
      showVoted: true
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
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: var(--spacer);
    padding: var(--spacer);
  }

  .books-grid > * {
    width: 200px;
  }

  .stats-summary {
    padding: var(--spacer);
  }

  .page-subtitle--flex {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    align-items: center;
  }

  .toggler {
    width: 0.9em;
    margin-left: calc(var(--spacer) / 2);
    transition-property: transform;
    transition-duration: 0.2s;
  }

  .toggler--hidden {
    transform: rotate(-90deg);
  }
</style>
