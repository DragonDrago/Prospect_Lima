<template>
  <div class="messages">
    <div class="filter-left">
      <i class="icofont-chart-bar-graph"></i>
      <div class="filter-name">{{ $t("messages") }}</div>
    </div>
    <div class="messages-page">
      <div class="messages-page__inner">
        <div
          class="messages-menu"
          :class="width < 991 && isMobileOpen ? 'messages-menu-hide' : ''"
        >
          <div class="messages-menu__top">
            <div class="navbar-search">
              <input type="search" :placeholder="$t('search') + '...'" />
              <span class="icofont icofont-search"></span>
            </div>
            <button type="button" class="messages-menu__btn">
              {{ $t("create_a_group") }}
            </button>
          </div>
          <div class="messages-contacts">
            <div class="messages-contacts__list" @click="getInside">
              <div class="messages-contacts__left">
                <span class="messages-contacts__status"></span>
                <div class="messages-contacts__name">Азиза</div>
              </div>
              <div class="messages-contacts__num">5</div>
            </div>
            <div class="messages-contacts__list" @click="getInside">
              <!-- active-list -->
              <div class="messages-contacts__left">
                <span class="messages-contacts__status active"></span>
                <div class="messages-contacts__name">Сергей</div>
              </div>
            </div>
            <div class="messages-contacts__list" @click="getInside">
              <div class="messages-contacts__left">
                <i class="icofont-people"></i>
                <div class="messages-contacts__name">
                  {{ $t("department") }}
                </div>
              </div>
              <div class="messages-contacts__num">5</div>
            </div>
          </div>
        </div>
        <div
          class="messages-main"
          :class="width < 991 && isMobileOpen ? 'messages-show' : ''"
        >
          <button
            class="back"
            @click="isMobileOpen = false"
            v-if="width < 991 && isMobileOpen"
          >
            <i class="icofont-rounded-left"></i>
          </button>
          <div class="messages-main__title">
            {{ $t("today") }}
          </div>
          <div class="messages-display">
            <div class="messages-itself guest">
              <div class="messages-itself__top">
                <div class="messages-itself__inner">
                  <img src="../assets/image/profile.jpg" alt="" />
                  <div class="messages-itself__name">Мадина</div>
                </div>
                <div class="messages-itself__time">18:00</div>
              </div>
              <div class="messages-itself__body">
                сегодня я не смогу выйти на работу, плохо себя чувствую
              </div>
            </div>
            <div class="messages-itself host">
              <div class="messages-itself__top">
                <div class="messages-itself__inner">
                  <img src="../assets/image/profile.jpg" alt="" />
                  <div class="messages-itself__name">Севара</div>
                </div>
                <div class="messages-itself__time">18:01</div>
              </div>
              <div class="messages-itself__body">
                Хорошо выздоравливайте, завтра ждём Вас
              </div>
            </div>
            <div class="messages-form">
              <button class="messages-form__btn file">
                <i class="icofont-clip"></i>
                <input type="file" />
              </button>
              <div class="navbar-search">
                <input type="text" :placeholder="$t('write_a_message')" />
              </div>
              <button class="messages-form__btn send">
                <i class="icofont-location-arrow"></i>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { useWindowSize } from "@vueuse/core";
import { onMounted, ref } from "@vue/composition-api";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import axios from "axios";
export default {
  setup() {
    const { width } = useWindowSize();
    const isMobileOpen = ref(false);
    const getInside = () => {
      if (width.value < 991) {
        isMobileOpen.value = true;
      }
    };

    onMounted(async () => {
      const { data } = await axios.get("/chats/all", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      console.log(data);
    });

    // onMounted(async () => {
    //   const connection = new HubConnectionBuilder()
    //     .withUrl("https://zbot.uz/chatHub")
    //     .configureLogging(LogLevel.Information)
    //     .build();

    //   async function start() {
    //     try {
    //       await connection.start();
    //       console.log("SignalR Connected.");
    //     } catch (err) {
    //       console.log(err);
    //       setTimeout(start, 5000);
    //     }
    //   }
    //   connection.onclose(async () => {
    //     await start();
    //   });

    //   connection.on("ReceiveMessage", (user, message) => {
    //     console.log(user, message);
    //   });
    //   // Start the connection.
    //   start();
    // });
    return {
      width,
      getInside,
      isMobileOpen,
    };
  },
};
</script>

<style></style>
