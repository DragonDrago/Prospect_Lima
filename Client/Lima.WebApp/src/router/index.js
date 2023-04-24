import Vue from "vue";
import VueRouter from "vue-router";
import store from "../store/";

Vue.use(VueRouter);

const grants = JSON.parse(localStorage.getItem("GRANTS")) || [];

const routes = [
  {
    path: "/login",
    name: "login",
    component: () => import("@/views/Login.vue"),
    meta: {
      layout: "empty",
      auth: false,
    },
  },
  {
    path: "/",
    name: "Home",
    component: () => import("@/views/Home.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
  {
    path: "/booking",
    name: "booking",
    component: () => import("@/views/Booking.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("SALES_VIEW"),
    },
  },
  {
    path: "/shipping",
    name: "shipping",
    component: () => import("@/views/Shipping.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("SALES_VIEW"),
    },
  },
  {
    path: "/remains",
    name: "remains",
    component: () => import("@/views/Remains.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("SALES_VIEW"),
    },
  },
  {
    path: "/visit-doctor",
    name: "visit-doctor",
    component: () => import("@/views/VisitDoctor.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("VISITS_DOCTOR_VIEW"),
    },
  },
  {
    path: "/visit-pharmacy",
    name: "visit-pharmacy",
    component: () => import("@/views/VisitPharmacy.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("VISITS_PHARMACY_DISTRIBUTORS_VIEW"),
    },
  },
  {
    path: "/visit-wholesaler",
    name: "visit-wholesaler",
    component: () => import("@/views/VisitWholesaler.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("VISITS_PHARMACY_DISTRIBUTORS_VIEW"),
    },
  },
  {
    path: "/list-doctors",
    name: "list-doctors",
    component: () => import("@/views/ListDoctors.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("DOCTORS_VIEW"),
    },
  },
  {
    path: "/list-lpu",
    name: "list-lpu",
    component: () => import("@/views/ListLpu.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("LPU_VIEW"),
    },
  },
  {
    path: "/list-pharmacy",
    name: "list-pharmacy",
    component: () => import("@/views/ListPharmacy.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("PHARMACY_DISTRIBUTORS_VIEW"),
    },
  },
  {
    path: "/list-wholesalers",
    name: "list-wholesalers",
    component: () => import("@/views/ListWholesalers.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("PHARMACY_DISTRIBUTORS_VIEW"),
    },
  },
  {
    path: "/list-goods",
    name: "list-goods",
    component: () => import("@/views/ListGoods.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("DRUGS_VIEW"),
    },
  },
  {
    path: "/list-series",
    name: "list-series",
    component: () => import("@/views/ListSeries.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("DRUGS_VIEW"),
    },
  },
  {
    path: "/calendar",
    name: "calendar",
    component: () => import("@/views/Calendar.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("EVENTS_VIEW"),
    },
  },
  {
    path: "/map",
    name: "map",
    component: () => import("@/views/Map.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("MAP_VIEW"),
    },
  },
  {
    path: "/roles-and-rules",
    name: "roles-and-rules",
    component: () => import("@/views/RolesAndRules.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("SETTINGS_VIEW"),
    },
  },
  {
    path: "/staff",
    name: "staff",
    component: () => import("@/views/Staff.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("SETTINGS_VIEW"),
    },
  },
  {
    path: "/branches",
    name: "branches",
    component: () => import("@/views/Branches.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("SETTINGS_VIEW"),
    },
  },
  {
    path: "/departments",
    name: "departments",
    component: () => import("@/views/Departments.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("SETTINGS_VIEW"),
    },
  },
  {
    path: "/prepayments-settings",
    name: "prepayments-settings",
    component: () => import("@/views/PrepaymentsSettings.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("SETTINGS_VIEW"),
    },
  },
  {
    path: "/presentation",
    name: "presentation",
    component: () => import("@/views/Presentation.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
  {
    path: "/messages",
    name: "messages",
    component: () => import("@/views/Messages.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
  {
    path: "/notifications",
    name: "notifications",
    component: () => import("@/views/Notifications.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
  {
    path: "/profile",
    name: "profile",
    component: () => import("@/views/Profile.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
  {
    path: "/reports",
    name: "reports",
    component: () => import("@/views/Reports.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
  {
    path: "/reports-region",
    name: "reports-region",
    component: () => import("@/views/ReportsRegion.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
  {
    path: "/reports-branches",
    name: "reports-branches",
    component: () => import("@/views/ReportsBranches.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
  {
    path: "/reports-staff",
    name: "reports-staff",
    component: () => import("@/views/ReportsStaff.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
  {
    path: "/settings",
    name: "settings",
    component: () => import("@/views/Settings.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: grants.includes("SETTINGS_VIEW"),
    },
  },
  {
    path: "*",
    component: () => import("@/views/PageNotFound.vue"),
    meta: {
      layout: "main",
      auth: true,
      permission: true,
    },
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
  store,
  linkActiveClass: "active",
  linkExactActiveClass: "active",
});

router.beforeEach((to, from, next) => {
  const requireAuth = to.meta.auth;
  const permission = to.meta.permission;
  if (requireAuth && store.getters["auth/isAuthenticated"]) {
    if (permission) {
      next();
    }
  } else if (requireAuth && !store.getters["auth/isAuthenticated"]) {
    next("/login");
  } else {
    next();
  }
});

export default router;
